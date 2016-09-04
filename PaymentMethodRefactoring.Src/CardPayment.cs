namespace PaymentMethodRefactoring.Src
{
    public class CardPayment : IPayment
    {
        public void Execute(PaymentDetails paymentDetails, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            paymentProvider.AuthorisePayment(paymentDetails.Amount, paymentDetails.OrderId, paymentDetails.PaymentMethod);

            var cardTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
            transactionRepo.Save(cardTransaction);
        }
    }
}