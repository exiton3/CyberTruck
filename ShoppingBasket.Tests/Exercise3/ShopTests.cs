using Moq;
using NUnit.Framework;

namespace ShoppingBasket.Tests.Exercise3
{
    // 1. - Payment accepted, in stock order placed
    // 2. - Payment rejected - order not placed give error
    // 3. - We cannot place order when CD out of stock

    [TestFixture]
    public class ShopTests
    {
        private CD _cd;
        private Mock<IPayment> _payment;
        private Mock<IOrder> _order;

        [SetUp]
        public void SetUp()
        {
            _order = new Mock<IOrder>();

            _payment = new Mock<IPayment>();

            _cd = new CD(_order.Object, _payment.Object);
        }

        [Test]
        public void BuyCD_PaymentAccepted_OrderPlaced()
        {
            _payment.Setup(x => x.AcceptPayment()).Returns(true);

            _cd.Buy();

            _order.Verify(x => x.PlaceOrder());
        }

        [Test]
        public void BuCD_PaymentRejected_OrderNotPlaced()
        {
            _payment.Setup(x => x.AcceptPayment()).Returns(false);

            _cd.Buy();

            _order.Verify(x => x.PlaceOrder(), Times.Never);
        }
    }
}