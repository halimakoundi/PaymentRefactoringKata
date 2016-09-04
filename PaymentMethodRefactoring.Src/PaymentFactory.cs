namespace PaymentMethodRefactoring.Src
{
    public class PaymentFactory
    {
        public static IPayment PaymentFor(PaymentDetails paymentDetails)
        {
            IPayment payment = null;
            switch (paymentDetails.PaymentMethod)
            {
                case "check":
                    payment = new CheckPayment();
                    break;
                case "card":
                    payment = new CardPayment();
                    break;
                case "direct-debit":
                    payment = new DirectDebitPayment();
                    break;
            }
            return payment;
        }
    }
}