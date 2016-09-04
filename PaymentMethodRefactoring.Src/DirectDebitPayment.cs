namespace PaymentMethodRefactoring.Src
{
    public class DirectDebitPayment : IPayment
    {
        public void Execute(PaymentDetails paymentDetails, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            paymentProvider.AuthorisePayment(paymentDetails.Amount, paymentDetails.OrderId, paymentDetails.PaymentMethod);

            var directDebitTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
            transactionRepo.Save(directDebitTransaction);
        }
    }
}