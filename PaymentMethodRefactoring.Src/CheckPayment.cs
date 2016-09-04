namespace PaymentMethodRefactoring.Src
{
    public class CheckPayment
    {
        public void ExecuteCheckPayment(Payment payment, TransactionRepo transactionRepo)
        {
            var cashTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(cashTransaction);
        }
    }
}