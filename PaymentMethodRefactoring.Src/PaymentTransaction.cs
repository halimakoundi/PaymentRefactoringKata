namespace PaymentMethodRefactoring.Src
{
    public class PaymentTransaction
    {
        private readonly string _orderId;
        private readonly PaymentInfo _paymentInfo;

        public PaymentTransaction(string orderId, PaymentInfo paymentInfo)
        {
            _orderId = orderId;
            _paymentInfo = paymentInfo;
        }
    }
}