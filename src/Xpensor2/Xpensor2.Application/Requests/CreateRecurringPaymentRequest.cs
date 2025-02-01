namespace Xpensor2.Application.Requests
{
    public class CreateRecurringPaymentRequest
    {
        public required string UserName { get; set; }
        public required string PaymentDescription { get; set; }
        public required decimal NominalValue { get; set; }
        public required int DueDay { get; set; }
    }
}
