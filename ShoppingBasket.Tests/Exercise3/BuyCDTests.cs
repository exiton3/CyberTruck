using Moq;
using NUnit.Framework;

namespace ShoppingBasket.Tests.Exercise3
{
    
    // 1.   CD In stock - book CD in warehouse and process payment
    // 2.   CD out of stock don't process payment return Error result
    // 3. - Payment accepted place order and return Success result
    // 4.   Payment rejected don't place order and Restock CD in warehouse and Return Error result
    // 5. - Order placed notify charts about sold CD Info

    [TestFixture]
    public class BuyCDTests
    {
        private CD _cd;
        private Mock<IPayment> _payment;
        private Mock<IOrder> _order;
        private Mock<IWarehouse> _warehouse;
        private Mock<ICharts> _charts;

        [SetUp]
        public void SetUp()
        {
            _order = new Mock<IOrder>();

            _payment = new Mock<IPayment>();
            _warehouse = new Mock<IWarehouse>();
            _charts = new Mock<ICharts>();

            _cd = new CD(_order.Object, _payment.Object, _warehouse.Object, _charts.Object);
        }

        [Test]
        public void CDOutOfStock_NoPaymentsProcessed()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(false);

            var result = _cd.Buy(1);

            _payment.Verify(x => x.AcceptPayment(), Times.Never);
        }


        [Test]
        public void CDOutOfStock_ReturnErrorResult()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(false);

            OrderResult result = _cd.Buy(1);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("CD out of stock"));
        }


        [Test]
        public void CdInStock_BookCDInWarehouse()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            var quantity = 1;

            _cd.Buy(quantity);

           _warehouse.Verify(x=>x.BookCd(_cd,quantity));
        }

        [Test]
        public void CdInStock_ProcessPayment()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            var quantity = 1;

            _cd.Buy(quantity);

            _payment.Verify(x => x.AcceptPayment());
        }

        [Test]
        public void PaymentIsRejected_ReturnErrorResult()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            var quantity = 1;

            var result = _cd.Buy(quantity);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Payment cannot be processed"));
        }

        [Test]
        public void PaymentIsRejected_RestockCDInWarehouse()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            var quantity = 1;

            var result = _cd.Buy(quantity);

            _warehouse.Verify(x => x.RestockCd(_cd, quantity));
        }

        [Test]
        public void PaymentRejected_OrderNotPlaced()
        {
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            _warehouse.Setup(x => x.IsInStock()).Returns(false);

            _cd.Buy(1);

            _order.Verify(x => x.PlaceOrder(), Times.Never);
        }

        [Test]
        public void PaymentAccepted_OrderPlaced()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            _cd.Buy(1);

            _order.Verify(x => x.PlaceOrder(), Times.Once);
        }

        [Test]
        public void PaymentAccepted_ReturnSuccessResult()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            var result = _cd.Buy(1);

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void CDSoldOrderPlaced_NotifyChartsAboutSalesWithCDInfo()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(true);
            _cd.Title = "title";
            _cd.Artist = "artist";

            _cd.Buy(10);

            _charts.Verify(x => x.Notify("title", "artist", 10));
        }

        [Test]
        public void CDNotSoldOrderPlaced_DoesNotNotifyChartsAboutSalesWithCDInfo()
        {
            _warehouse.Setup(x => x.IsInStock()).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            _cd.Title = "title";
            _cd.Artist = "artist";

            _cd.Buy(10);

            _charts.Verify(x => x.Notify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}