namespace Xpensor2.Domain.Models;

public class ExecutedPayment
{
    public required string PaymentMethod { get; set; }
    public required DateTime PaidDate { get; set; }
    public required DateTime PaymentDueDate { get; set; }
    public required User PaidBy { get; set; }
}
