﻿using System;

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

                    break;
                case "card":
                    _paymentProvider.AuthorisePayment(amount, orderId, paymentMethod);
                    PaymentTransaction transaction = PaymentTransaction.With(paymentMethod, amount, orderId);
                    _transactionRepo.Save(transaction);
                    break;
                case "direct-debit":
                    break;
            }
            //TODO here we will extract method below into sendConfirmationEmail method
            var orderConfirmationEmail  = _emailGateway.NewEmailFor(orderId, customerId, paymentMethod);
            _emailGateway.Send(orderConfirmationEmail);
        }
        
    }
}
