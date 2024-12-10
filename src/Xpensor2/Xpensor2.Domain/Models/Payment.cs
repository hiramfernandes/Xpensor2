using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Models;

public class Payment
{
    public Payment(string description, User owner, decimal nominalValue, int dueDay)
    {
        Id = Guid.NewGuid();
        Description = description;
        Owner = owner;
        NominalValue = nominalValue;
        DueDay = dueDay;
        PaymentType = PaymentType.Recurring;
        Recurrence = PaymentRecurrence.Monthly;
    }

    public Payment(string description, User owner, decimal nominalValue, int dueDay, long installmentNumber, DateTime? startDate)
        : this(description, owner, nominalValue, dueDay)
    {
        InstallmentNumber = installmentNumber;
        StartDate = startDate;
        PaymentType = PaymentType.Installment;
    }

    public Guid Id { get; private set; }
    public string Description { get; init; }
    public decimal? NominalValue { get; set; }
    public DateTime DueDate { get; set; }
    public int DueDay { get; set; }
    public PaymentRecurrence Recurrence { get; set; }
    public int? NumberOfInstallments { get; set; }
    public long? InstallmentNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public PaymentType PaymentType { get; private set; }
    public IList<ExecutedPayment> ExecutedPayments { get; private set; } = [];
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public User? Owner { get; set; }

    public void ExecutePayment(ExecutedPayment executedPayment)
    {
        ExecutedPayments.Add(executedPayment);
    }
}
