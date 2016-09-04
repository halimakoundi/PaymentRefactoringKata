using NSubstitute;
using NUnit.Framework;
using PaymentMethodRefactoring.Src;

namespace PaymentMethodRefactoring.Tests
{
    [TestFixture]
    public class PaymentServiceShould
    {
        private PaymentService _paymentService;
        private IPaymentProvider _paymentProvider;
        private IEmailGateway _emailGateway;
        private TransactionRepo _transactionRepo;
        private string _customerId;

        [SetUp]
        public void SetUp()
        {
            _paymentProvider = Substitute.For<IPaymentProvider>();
            _emailGateway = Substitute.For<IEmailGateway>();
            _transactionRepo = Substitute.For<TransactionRepo>();
            _paymentService = new PaymentService(_paymentProvider, _emailGateway, _transactionRepo);
            _customerId = "Cust-2345";
        }

        [TestCase("check", "123-324-456", 13.21)]
        public void save_transaction_and_send_confirmation_when_payment_by_check(string paymentMethod, string orderId, decimal amount)
        {
            var email = new OrderConfirmationEmail(orderId, _customerId, paymentMethod);
            _emailGateway.NewEmailFor(orderId, _customerId, paymentMethod).Returns(email);
            var transaction = new PaymentTransaction(orderId, amount, paymentMethod);

            _paymentService.Pay(amount, _customerId, orderId, paymentMethod);

            _transactionRepo.ReceivedWithAnyArgs().Save(transaction);
            _emailGateway.Received().Send(email);
        }

        [TestCase("card", "123-324-456", 153.90)]
        [TestCase("direct-debit", "123-324-458", 74.89)]
        public void save_transaction_and_send_confirmation_when_payment_authorised(string paymentMethod, string orderId, decimal amount)
        {
            var email = new OrderConfirmationEmail(orderId, _customerId, paymentMethod);
            _emailGateway.NewEmailFor(orderId, _customerId, paymentMethod).Returns(email);
            var transaction = new PaymentTransaction(orderId, amount,paymentMethod);

            _paymentService.Pay(amount,  _customerId, orderId, paymentMethod);

            _paymentProvider.Received().AuthorisePayment(amount, orderId, paymentMethod);
            _transactionRepo.ReceivedWithAnyArgs().Save(transaction);
            _emailGateway.Received().Send(email);
        }

    }
}
