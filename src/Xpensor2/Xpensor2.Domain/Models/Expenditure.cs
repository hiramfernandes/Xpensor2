namespace Xpensor2.Domain.Models;

public class Expenditure
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public string? GeneralInfo { get; set; }
    public ExecutedPayment? ExecutedPayment { get; private set; }

    public void Pay(ExecutedPayment executedPayment)
    {
        ExecutedPayment = executedPayment;
    }
}
