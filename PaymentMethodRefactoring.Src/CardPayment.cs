namespace PaymentMethodRefactoring.Src
{
    public class CardPayment
    {
        public void ExecuteCardPayment(Payment payment, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

            var cardTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(cardTransaction);
        }
    }
}