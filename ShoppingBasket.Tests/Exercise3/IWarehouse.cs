namespace ShoppingBasket.Tests.Exercise3
{
    public interface IWarehouse
    {
        bool IsInStock(BuyCDRequest request);
        void BookCd(BuyCDRequest request);
        void RestockCd(BuyCDRequest request);
    }
}