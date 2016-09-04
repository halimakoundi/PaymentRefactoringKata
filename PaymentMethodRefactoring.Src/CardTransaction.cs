namespace PaymentMethodRefactoring.Src
{
    public class CardTransaction : PaymentTransaction
    {
        public CardTransaction(string orderId, PaymentInfo paymentInfo) : base(orderId, paymentInfo)
        {
        }
    }
}