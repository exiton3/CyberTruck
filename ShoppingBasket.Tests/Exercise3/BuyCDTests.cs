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
        private MusicStore _musicStore;
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
            _warehouse.Setup(x => x.IsInStock(It.IsAny<BuyCDRequest>())).Returns(true);
            _musicStore = new MusicStore(_order.Object, _payment.Object, _warehouse.Object, _charts.Object);
        }

        [Test]
        public void CDOutOfStock_NoPaymentsProcessed()
        {
            var cdRequest = new BuyCDRequest(1);

            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(false);

            _musicStore.Buy(cdRequest);

            _payment.Verify(x => x.AcceptPayment(), Times.Never);
        }


        [Test]
        public void CDOutOfStock_ReturnErrorResult()
        {
            var cdRequest = new BuyCDRequest(1);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(false);

            OrderResult result = _musicStore.Buy(cdRequest);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("CD out of stock"));
        }


        [Test]
        public void CdInStock_BookCDInWarehouse()
        {
            var cdRequest = new BuyCDRequest(1);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);

            _musicStore.Buy(cdRequest);

           _warehouse.Verify(x=>x.BookCd(cdRequest));
        }

        [Test]
        public void CdInStock_ProcessPayment()
        {
            var cdRequest = new BuyCDRequest(1);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);

            _musicStore.Buy(cdRequest);

            _payment.Verify(x => x.AcceptPayment());
        }

        [Test]
        public void PaymentIsRejected_ReturnErrorResult()
        {
             var cdRequest = new BuyCDRequest(1);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);

            var result = _musicStore.Buy(cdRequest);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Payment cannot be processed"));
        }

        [Test]
        public void PaymentIsRejected_RestockCDInWarehouse()
        {
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            var cdRequest = new BuyCDRequest(1);

            _musicStore.Buy(cdRequest);

            _warehouse.Verify(x => x.RestockCd(cdRequest));
        }

        [Test]
        public void PaymentRejected_OrderNotPlaced()
        {
            var cdRequest = new BuyCDRequest(1);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(false);

            _musicStore.Buy(cdRequest);

            _order.Verify(x => x.PlaceOrder(), Times.Never);
        }

        [Test]
        public void PaymentAccepted_OrderPlaced()
        {
            var cdRequest = new BuyCDRequest(1);
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            _musicStore.Buy(cdRequest);

            _order.Verify(x => x.PlaceOrder(), Times.Once);
        }

        [Test]
        public void PaymentAccepted_ReturnSuccessResult()
        {
          
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            var result = _musicStore.Buy(new BuyCDRequest(1));

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void CDSoldOrderPlaced_NotifyChartsAboutSalesWithCDInfo()
        {
            var cdRequest = new BuyCDRequest(10) { Title = "title", Artist = "artist" };
            
            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            _musicStore.Buy(cdRequest);

            _charts.Verify(x => x.Notify("title", "artist", 10));
        }

        [Test]
        public void CDNotSoldOrderPlaced_DoesNotNotifyChartsAboutSalesWithCDInfo()
        {
            var cdRequest = new BuyCDRequest(10);

            _warehouse.Setup(x => x.IsInStock(cdRequest)).Returns(true);
            _payment.Setup(x => x.AcceptPayment()).Returns(false);

            _musicStore.Buy(cdRequest);

            _charts.Verify(x => x.Notify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}