﻿namespace PaymentMethodRefactoring.Src
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
                    var checkTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(checkTransaction);
                    break;
                case "card":
                    _paymentProvider.AuthorisePayment(amount, orderId, paymentMethod, customerId);

                    var cardTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(cardTransaction);
                    break;
                case "direct-debit":
                    _paymentProvider.AuthorisePayment(amount, orderId, paymentMethod,customerId);

                    var directDebitTransaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(directDebitTransaction);
                    break;
            }

            var orderConfirmationEmail = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
    }
}
