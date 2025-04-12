namespace Xpensor2.Application.Requests
{
    public class ExecutePaymentRequest
    {
        public required string ExpenditureId { get; set; }
        public required string PaymentMethod { get; set; }
        public required decimal PaidValue { get; set; }
        public required DateTime PaidDate { get; set; }
        public required string PaidBy { get; set; }
    }
}
