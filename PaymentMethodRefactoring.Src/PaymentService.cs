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
            switch (paymentMethod)
            {
                case "check":

                    break;
                case "card":
                    var transaction = new PaymentTransaction(orderId, paymentInfo);
                    //TODO do some checks here
                    _paymentProvider.MakePayment(transaction);
                    break;
                case "direct-debit":
                    break;
            }
            //TODO here we will extract method below into sendConfirmationEmail method
            var orderConfirmationEmail  = new OrderConfirmationEmail();
            _emailGateway.Send(orderConfirmationEmail);
        }
    }
}
