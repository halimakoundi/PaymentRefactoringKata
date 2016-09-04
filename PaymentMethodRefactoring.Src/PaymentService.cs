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
            switch (paymentMethod)
            {
                case "check":
                    var cashTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(cashTransaction);
                    break;
                case "card":
                    _paymentProvider.AuthorisePayment(amount, orderId, paymentMethod);

                    var cardTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(cardTransaction);
                    break;
                case "direct-debit":
                    _paymentProvider.AuthorisePayment(amount, orderId, paymentMethod);

                    var directDebitTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(directDebitTransaction);
                    break;
            }

            var orderConfirmationEmail  = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
        
    }
}
