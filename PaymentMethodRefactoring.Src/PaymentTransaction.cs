namespace PaymentMethodRefactoring.Src
{
    public class PaymentTransaction
    {
        private readonly string _orderId;
        private readonly decimal _amount;
        private readonly string _paymentMethod;

        public PaymentTransaction(string orderId, decimal amount, string paymentMethod)
        {
            _orderId = orderId;
            _amount = amount;
            _paymentMethod = paymentMethod;
        }

        public static PaymentTransaction With(string paymentMethod, decimal amount, string orderId)
        {
            return new PaymentTransaction(orderId, amount, paymentMethod);
        }
    }
}