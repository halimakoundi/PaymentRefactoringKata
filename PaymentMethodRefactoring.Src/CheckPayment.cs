namespace PaymentMethodRefactoring.Src
{
    public class CheckPayment
    {
        public void Execute(Payment payment, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            var cashTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(cashTransaction);
        }
    }
}