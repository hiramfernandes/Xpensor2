using MongoDB.Bson.Serialization.Attributes;

namespace Xpensor2.Domain.Models;

public class Expenditure
{
    public Expenditure(Payment payment, DateTime dueDate, string description, string generalInfo)
    {
        Id = Guid.NewGuid().ToString();
        Payment = payment;
        DueDate = dueDate;
        Name = description;
        GeneralInfo = generalInfo;
    }

    public string Id { get; set; }

    [BsonIgnore]
    public Payment Payment { get; set; }
    public string? PaymentId => Payment.Id;
    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public string? GeneralInfo { get; set; }
    public long? InstallmentNumber { get; set; }
    public long? TotalInstallments { get; set; }
    public ExecutedPayment? ExecutedPayment { get; private set; }

    public void Pay(ExecutedPayment executedPayment)
    {
        ExecutedPayment = executedPayment;
    }
}
