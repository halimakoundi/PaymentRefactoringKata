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

        public class PaymentDetails
        {
            public PaymentDetails(decimal amount, string orderId, string paymentMethod)
            {
                Amount = amount;
                OrderId = orderId;
                PaymentMethod = paymentMethod;
            }

            public decimal Amount { get; }

            public string OrderId { get; }

            public string PaymentMethod { get; }
        }

        private void ExecutePayment(PaymentDetails paymentDetails)
        {
            switch (paymentDetails.PaymentMethod)
            {
                case "check":
                    var cashTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
                    _transactionRepo.Save(cashTransaction);
                    break;
                case "card":
                    _paymentProvider.AuthorisePayment(paymentDetails.Amount, paymentDetails.OrderId, paymentDetails.PaymentMethod);

                    var cardTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
                    _transactionRepo.Save(cardTransaction);
                    break;
                case "direct-debit":
                    _paymentProvider.AuthorisePayment(paymentDetails.Amount, paymentDetails.OrderId, paymentDetails.PaymentMethod);

                    var directDebitTransaction = PaymentTransaction.With(paymentDetails.PaymentMethod, paymentDetails.Amount, paymentDetails.OrderId);
                    _transactionRepo.Save(directDebitTransaction);
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
