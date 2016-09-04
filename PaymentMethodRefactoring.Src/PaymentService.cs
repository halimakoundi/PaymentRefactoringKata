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
            ExecutePayment(new Payment(amount, orderId, paymentMethod));

            SendConfirmationEmail(customerId, orderId, paymentMethod);
        }

        private void ExecutePayment(Payment payment)
        {
            switch (payment.PaymentMethod)
            {
                case "check":
                    CheckPayment.ExecuteCheckPayment(payment, _transactionRepo);
                    break;
                case "card":
                    CardPayment.ExecuteCardPayment(payment, _paymentProvider, _transactionRepo);
                    break;
                case "direct-debit":
                    DirectDebitPayment.ExecuteDirectDebitPayment(payment, _paymentProvider, _transactionRepo);
                    break;
            }
        }

        private void SendConfirmationEmail(string customerId, string orderId, string paymentMethod)
        {
            var orderConfirmationEmail = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
    }
}
