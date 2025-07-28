namespace Xpensor2.Application.Requests;

public class UpdateExpenseRequest
{
        public required string ExpenseId { get; set; }
        public  required string Description { get; set; }
        public required decimal ExpenseValue { get; set; }
        public string? SpecialInstruction { get; set; }
        public DateTime DueDate { get; set; }
}
