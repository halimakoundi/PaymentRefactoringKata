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

        public void Execute(IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            var cashTransaction = PaymentTransaction.With(PaymentMethod, Amount, OrderId);
            transactionRepo.Save(cashTransaction);
        }
    }
}