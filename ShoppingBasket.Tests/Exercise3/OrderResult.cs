using System;

namespace ShoppingBasket.Tests.Exercise3
{
    public class OrderResult
    {
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage { get; set; }
        

        public static OrderResult OK => new OrderResult { ErrorMessage = String.Empty};
    }
}