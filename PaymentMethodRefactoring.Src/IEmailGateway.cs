namespace PaymentMethodRefactoring.Src
{
    public interface IEmailGateway
    {
        void Send(Email email);
        Email NewEmailFor(string orderId, string paymentMethod);
    }
}