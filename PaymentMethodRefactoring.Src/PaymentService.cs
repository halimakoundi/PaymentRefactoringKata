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
                    ExecuteCheckPayment(payment, _transactionRepo);
                    break;
                case "card":
                    ExecuteCardPayment(payment, _paymentProvider, _transactionRepo);
                    break;
                case "direct-debit":
                    ExecuteDirectDebitPayment(payment);
                    break;
            }
        }

        public static void ExecuteCheckPayment(Payment payment, TransactionRepo transactionRepo)
        {
            var cashTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(cashTransaction);
        }

        private void ExecuteCardPayment(Payment payment, IPaymentProvider paymentProvider, TransactionRepo transactionRepo)
        {
            paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

            var cardTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            transactionRepo.Save(cardTransaction);
        }

        private void ExecuteDirectDebitPayment(Payment payment)
        {
            _paymentProvider.AuthorisePayment(payment.Amount, payment.OrderId, payment.PaymentMethod);

            var directDebitTransaction = PaymentTransaction.With(payment.PaymentMethod, payment.Amount, payment.OrderId);
            _transactionRepo.Save(directDebitTransaction);
        }

        private void SendConfirmationEmail(string customerId, string orderId, string paymentMethod)
        {
            var orderConfirmationEmail = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
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
}
