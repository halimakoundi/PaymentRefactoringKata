namespace PaymentMethodRefactoring.Src
{
    public class CheckPayment : IPayment
    {
        public void Execute(PaymentDetails paymentDetails, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            var checkTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
            transactionRepo.Save(checkTransaction);
        }
    }
}