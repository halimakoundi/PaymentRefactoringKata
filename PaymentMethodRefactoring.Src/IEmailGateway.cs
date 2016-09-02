namespace PaymentMethodRefactoring.Src
{
    public interface IEmailGateway
    {
        void Send(Email email);
    }
}