﻿using NSubstitute;
using NUnit.Framework;
using PaymentMethodRefactoring.Src;

namespace PaymentMethodRefactoring.Tests
{
    [TestFixture]
    public class PaymentFeature1
    {
        private const string OrderId = "123-324-456";
        private PaymentService _paymentService;
        private PaymentProvider _paymentProvider;
        private IEmailGateway _emailGateway;

        [Test]
        public void pay_order_and_send_confirmation_email()
        {
            Substitute.For<UserAccountChecker>();
            _paymentProvider = Substitute.For<PaymentProvider>();
            _emailGateway = Substitute.For<IEmailGateway>();
            _paymentService = new PaymentService(_paymentProvider, _emailGateway);
            var paymentMethod = "card";
            Email email = new OrderConfirmationEmail();
            var transaction = new CardTransaction();

            var paymentInfo = new PaymentInfo();
            _paymentService.Pay(OrderId, paymentMethod, paymentInfo);

            _paymentProvider.Received().MakePayment(transaction);
            _emailGateway.Received().Send(email);
        }
    }
}