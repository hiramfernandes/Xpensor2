namespace Xpensor2.Application.Requests
{
    public class CreateInstallmentPaymentRequest
    {
        public  required string PaymentDescription { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required decimal InstallmentValue { get; set; }
        public required int NumberOfInstallments { get; set; }
        public DateOnly StartDate { get; set; }
    }
}
