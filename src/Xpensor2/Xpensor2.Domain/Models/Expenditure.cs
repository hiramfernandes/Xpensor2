using MongoDB.Bson.Serialization.Attributes;

namespace Xpensor2.Domain.Models;

public class Expenditure
{
    public Expenditure(DateTime dueDate,
                       decimal value,
                       string description,
                       string generalInfo)
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
    public long? InstallmentNumber { get; set; }
    public long? TotalInstallments { get; set; }
    public ExecutedPayment? ExecutedPayment { get; private set; }

    public void Pay(ExecutedPayment executedPayment)
    {
        ExecutedPayment = executedPayment;
    }
}
