using NSubstitute;
using NUnit.Framework;
using PaymentMethodRefactoring.Src;

namespace PaymentMethodRefactoring.Tests
{
    [TestFixture]
    public class PaymentServiceShould
    {
        private PaymentService _paymentService;
        private PaymentProvider _paymentProvider;
        private IEmailGateway _emailGateway;

        [SetUp]
        public void SetUp()
        {
            _paymentProvider = Substitute.For<PaymentProvider>();
            _emailGateway = Substitute.For<IEmailGateway>();
            _paymentService = new PaymentService(_paymentProvider, _emailGateway);
        }

        [TestCase("card", "123-324-456")]
        public void pay_order_and_send_confirmation_email(string paymentMethod, string orderId)
        {
            var email = new OrderConfirmationEmail(orderId, paymentMethod);
            var paymentInfo = new PaymentInfo();
            _emailGateway.NewEmailFor(orderId, paymentMethod).Returns(email);

            _paymentService.Pay(orderId, paymentMethod, paymentInfo);

            _paymentProvider.Received().MakePayment(orderId, paymentInfo);
            _emailGateway.Received().Send(email);
        }
    }
}
