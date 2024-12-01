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
        Recurrence = PaymentRecurrence.Monthly;
    }

    public Guid Id { get; private set; }
    public string Description { get; init; }
    public decimal? NominalValue { get; set; }
    public DateTime DueDate { get; set; }
    public int DueDay { get; set; }
    public PaymentRecurrence Recurrence { get; set; }
    public IList<ExecutedPayment> ExecutedPayments { get; private set; } = [];
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public User? Owner { get; set; }

    public void ExecutePayment(ExecutedPayment executedPayment)
    {
        ExecutedPayments.Add(executedPayment);
    }
}
