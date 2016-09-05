namespace PaymentMethodRefactoring.Src
{
    public interface IPaymentProvider
    {
        void AuthorisePayment(decimal amount, string id, string orderId, string paymentMethod);

    }
}