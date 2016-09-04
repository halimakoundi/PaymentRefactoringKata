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

        public class Payment
        {
            public Payment(decimal amount, string orderId, string paymentMethod)
            {
                Amount = amount;
                OrderId = orderId;
                PaymentMethod = paymentMethod;
            }

            public decimal Amount { get; }

            public string OrderId { get; }

            public string PaymentMethod { get; }
        }

        private void ExecutePayment(Payment payment)
        {
            switch (payment.PaymentMethod)
            {
                case "check":
                    var cashTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
                    _transactionRepo.Save(cashTransaction);
                    break;
                case "card":
                    _paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

                    var cardTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
                    _transactionRepo.Save(cardTransaction);
                    break;
                case "direct-debit":
                    _paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

                    var directDebitTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
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
