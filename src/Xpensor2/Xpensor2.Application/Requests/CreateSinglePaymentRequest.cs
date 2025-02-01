namespace Xpensor2.Application.Requests
{
    public class CreateSinglePaymentRequest
    {
        public required string UserName { get; set; }
        public required string UserId { get; set; }
        public required string Description { get; set; }
        public required decimal NominalValue { get; set; }
        public required DateOnly DueDate { get; set; }
    }
}
