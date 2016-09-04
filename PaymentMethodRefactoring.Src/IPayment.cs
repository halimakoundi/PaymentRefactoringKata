namespace PaymentMethodRefactoring.Src
{
    public interface IPayment
    {
        void Execute(PaymentDetails paymentDetails, IPaymentProvider paymentProvider, TransactionRepo transactionRepo);
    }
}