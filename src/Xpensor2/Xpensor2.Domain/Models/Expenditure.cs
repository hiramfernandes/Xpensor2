namespace Xpensor2.Domain.Models;

public class Expenditure
{
    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public string? GeneralInfo { get; set; }
    public DateTime? PaymentDate { get; set; }
}
