namespace ShoppingBasket.Tests.Exercise3
{
    public class CD
    {
        private readonly IOrder _order;
        private readonly IPayment _payment;

        public CD(IOrder order, IPayment payment)
        {
            _order = order;
            _payment = payment;
        }

        public void Buy()
        {
            if (_payment.AcceptPayment())
            {
                _order.PlaceOrder();
            }
        }
    }
}