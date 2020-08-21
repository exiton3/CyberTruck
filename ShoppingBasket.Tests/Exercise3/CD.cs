namespace ShoppingBasket.Tests.Exercise3
{
    public class CD
    {
        private readonly IOrder _order;
        private readonly IPayment _payment;
        private readonly IWarehouse _warehouse;
        private readonly ICharts _charts;

        public CD(IOrder order, IPayment payment, IWarehouse warehouse, ICharts charts)
        {
            _order = order;
            _payment = payment;
            _warehouse = warehouse;
            _charts = charts;
        }

        public string Title { get; set; }
        public string Artist { get; set; }

        public OrderResult Buy(int quantity)
        {
            if (!_warehouse.IsInStock())
            {
                return new OrderResult { ErrorMessage = "CD out of stock" };
            }

            _warehouse.BookCd(this, quantity);

            if (_payment.AcceptPayment())
            {
                _order.PlaceOrder();
                _charts.Notify(Title, Artist, quantity);
            }
            else
            {
                _warehouse.RestockCd(this, quantity);
                return new OrderResult { ErrorMessage = "Payment cannot be processed" };
            }

            return OrderResult.OK;
        }
    }
}