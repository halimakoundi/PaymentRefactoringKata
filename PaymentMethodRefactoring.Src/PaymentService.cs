namespace PaymentMethodRefactoring.Src
{
    public class PaymentService
    {
        private readonly IPaymentProvider _paymentProvider;
        private readonly IEmailGateway _emailGateway;
        private readonly TransactionRepo _transactionRepo;

        public PaymentService(IPaymentProvider paymentProvider, IEmailGateway emailGateway, TransactionRepo transactionRepo)
        {
            _paymentProvider = paymentProvider;
            _emailGateway = emailGateway;
            _transactionRepo = transactionRepo;
        }

        public void Pay(decimal amount, string customerId, string orderId, string paymentMethod)
        {
            ExecutePayment(new PaymentDetails(amount, orderId, paymentMethod));

            SendConfirmationEmail(customerId, orderId, paymentMethod);
        }

        private void ExecutePayment(PaymentDetails paymentDetails)
        {
            var payment = PaymentFor(paymentDetails);
            payment.Execute(paymentDetails, _paymentProvider, _transactionRepo);
        }

        private static IPayment PaymentFor(PaymentDetails paymentDetails)
        {
            IPayment payment = null;
            switch (paymentDetails.PaymentMethod)
            {
                case "check":
                    payment = new CheckPayment();
                    break;
                case "card":
                    payment = new CardPayment();
                    break;
                case "direct-debit":
                    payment = new DirectDebitPayment();
                    break;
            }
            return payment;
        }

        private void SendConfirmationEmail(string customerId, string orderId, string paymentMethod)
        {
            var orderConfirmationEmail = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
    }
}
