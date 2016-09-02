namespace PaymentMethodRefactoring.Src
{
    public class PaymentService
    {
        private readonly PaymentProvider _paymentProvider;
        private readonly IEmailGateway _emailGateway;

        public PaymentService(PaymentProvider paymentProvider, IEmailGateway emailGateway)
        {
            _paymentProvider = paymentProvider;
            _emailGateway = emailGateway;
        }

        public void Pay(string orderId, string paymentMethod, PaymentInfo paymentInfo)
        {
            var transaction = new PaymentTransaction();
            _paymentProvider.MakePayment(transaction);

            var orderConfirmationEmail  = new OrderConfirmationEmail();
            _emailGateway.Send(orderConfirmationEmail);
        }
    }
}
