namespace PaymentMethodRefactoring.Src
{
    internal interface IPayment
    {
        void Execute(PaymentDetails paymentDetails, IPaymentProvider paymentProvider, TransactionRepo transactionRepo);
    }
}