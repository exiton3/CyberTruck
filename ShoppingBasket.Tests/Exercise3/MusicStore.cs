namespace ShoppingBasket.Tests.Exercise3
{
    public class MusicStore
    {
        private readonly IOrder _order;
        private readonly IPayment _payment;
        private readonly IWarehouse _warehouse;
        private readonly ICharts _charts;

        public MusicStore(IOrder order, IPayment payment, IWarehouse warehouse, ICharts charts)
        {
            _order = order;
            _payment = payment;
            _warehouse = warehouse;
            _charts = charts;
        }

        public OrderResult Buy(BuyCDRequest request)
        {
            if (!_warehouse.IsInStock(request))
            {
                return new OrderResult { ErrorMessage = "CD out of stock" };
            }

            _warehouse.BookCd(request);

            if (_payment.AcceptPayment())
            {
                _order.PlaceOrder();
                _charts.Notify(request.Title, request.Artist, request.Quantity);

                return OrderResult.OK;
            }
            else
            {
                _warehouse.RestockCd(request);
                return new OrderResult { ErrorMessage = "Payment cannot be processed" };
            }
        }
    }
}