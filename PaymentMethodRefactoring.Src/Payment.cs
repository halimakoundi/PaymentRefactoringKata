namespace PaymentMethodRefactoring.Src
{
    public class Payment
    {
        public Payment(decimal amount, string orderId, string paymentMethod)
        {
            Amount = amount;
            OrderId = orderId;
            PaymentMethod = paymentMethod;
        }

        public decimal Amount { get; }

        public string OrderId { get; }

        public string PaymentMethod { get; }
    }
}