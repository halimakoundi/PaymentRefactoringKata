namespace PaymentMethodRefactoring.Src
{
    public class OrderConfirmationEmail : Email
    {
        private string orderId;
        private string paymentMethod;

        public OrderConfirmationEmail(string id, string orderId, string paymentMethod)
        {
            this.orderId = orderId;
            this.paymentMethod = paymentMethod;
        }
    }
}