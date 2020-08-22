namespace ShoppingBasket.Tests.Exercise3
{
    public class BuyCDRequest
    {
        public BuyCDRequest(int quantity)
        {
            Quantity = quantity;
        }

        public int Quantity { get; private set; }
        public string Artist { get; set; }
        public string Title { get; set; }
    }
}