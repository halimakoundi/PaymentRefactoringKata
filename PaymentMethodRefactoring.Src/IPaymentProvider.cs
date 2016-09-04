namespace PaymentMethodRefactoring.Src
{
    public interface IPaymentProvider
    {
        void AuthorisePayment(decimal amount, string orderId, string paymentMethod);

    }
}