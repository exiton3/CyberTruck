namespace ShoppingBasket.Tests.Exercise3
{
    public interface IWarehouse
    {
        bool IsInStock();
        void BookCd(CD cd, int quantity);
        void RestockCd(CD cd, in int quantity);
    }
}