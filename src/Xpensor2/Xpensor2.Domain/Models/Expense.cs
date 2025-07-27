namespace Xpensor2.Domain.Models;

public class Expense
{
    public Expense(DateTime dueDate,
                   decimal value,
                   string description,
                   string? generalInfo)
    {
        Id = Guid.NewGuid().ToString();
        DueDate = dueDate;
        Name = description;
        Value = value;
        GeneralInfo = generalInfo;
    }

    public string Id { get; set; }

    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Value { get; set; }
    public string? GeneralInfo { get; set; }
    public ExecutedPayment? ExecutedPayment { get; private set; }

    public void Pay(ExecutedPayment executedPayment)
    {
        ExecutedPayment = executedPayment;
    }
}
