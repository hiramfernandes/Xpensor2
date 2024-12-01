namespace Xpensor2.Domain.Models;

public class ExecutedPayment
{
    public required Payment Payment { get; set; }
    public required string PaymentMethod { get; set; }
    public required DateTime PaidDate { get; set; }
    public required User PaidBy { get; set; }
}
