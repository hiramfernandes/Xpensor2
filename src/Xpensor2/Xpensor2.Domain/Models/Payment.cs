using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Models;

public class Payment
{
    // Recurring Payment
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

    // Installment
    public Payment(string description, User owner, decimal nominalValue, int dueDay, int? numberOfInstallments, DateTime startDate)
        : this(description, owner, nominalValue, dueDay)
    {
        NumberOfInstallments = numberOfInstallments;
        StartDate = startDate;
        PaymentType = PaymentType.Installment;
    }

    // Single
    public Payment(string description, User owner, decimal nominalValue, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        Description = description;
        DueDate = dueDate;
        PaymentType = PaymentType.Single;
        NominalValue = nominalValue;
        Owner = owner;
        Recurrence = PaymentRecurrence.None;
    }

    public Guid Id { get; private set; }
    public string Description { get; init; }
    public decimal? NominalValue { get; set; }
    public DateTime DueDate { get; set; }
    public int DueDay { get; set; }
    public PaymentRecurrence Recurrence { get; set; }
    public DateTime? StartDate { get; set; }
    public int? NumberOfInstallments { get; set; }
    public PaymentType PaymentType { get; private set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public User Owner { get; set; }
}
