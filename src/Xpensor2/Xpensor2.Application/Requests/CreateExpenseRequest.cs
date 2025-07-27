namespace Xpensor2.Application.Requests
{
    public class CreateExpenseRequest
    {
        public  required string Description { get; set; }
        public required string UserId { get; set; }
        public required decimal ExpenseValue { get; set; }
        public string? SpecialInstruction { get; set; }
        public int NumberOfInstallments { get; set; }
        public DateTime DueDate { get; set; }
    }
}
