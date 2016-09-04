namespace PaymentMethodRefactoring.Src
{
    public class DirectDebitPayment
    {
        public void ExecuteDirectDebitPayment(Payment payment, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

            var directDebitTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(directDebitTransaction);
        }
    }
}