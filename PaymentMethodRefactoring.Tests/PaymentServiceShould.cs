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

        [TestCase("card", "123-324-456", 26.70)]
        public void pay_order_and_send_confirmation_email(string paymentMethod, string orderId, decimal amount)
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
