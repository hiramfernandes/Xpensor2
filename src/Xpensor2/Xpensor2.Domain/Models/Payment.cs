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


    public void AddExecutedPayment(ExecutedPayment executedPayment)
    {
        ExecutedPayments.Add(executedPayment);
    }
}

public enum PaymentRecurrence
{
    Monthly
}

public class ExecutedPayment
{
    public required Payment Payment { get; set; }
    public required string PaymentMethod { get; set; }
    public required DateTime PaidDate { get; set; }
    public required User PaidBy { get; set; }
}

public class PaymentSlice
{
    public PaymentSlice() => Payments = [];

    public List<Payment>? Payments { get; private set; }

    public void AddPayment(Payment payment)
    {
        Payments!.Add(payment);
    }

    public IEnumerable<Expenditure>? GenerateExpenditures(DateTime referenceDate)
    {
        // Regular Payments (no end date)
        return Payments!
            .Select(pmt => MapFrom(payment: pmt,
                                   month: referenceDate.Month,
                                   year: referenceDate.Year));
    }

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        return new Expenditure()
        {
            DueDate = new DateTime(year, month, payment.DueDay),
            Name = payment.Description,
            GeneralInfo = string.Empty
        };
    }
}

public class Expenditure
{
    public string? Name { get; set; }
    public DateTime DueDate { get; set; }
    public string? GeneralInfo { get; set; }
    public DateTime? PaymentDate { get; set; }
}

public class User
{
    public User(Guid id, string? name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; init; }
    public string? Name { get; init; }
    public IList<Payment> Payments { get; private set; } = [];

    public Payment CreatePayment(string description, decimal nominalValue, int dueDay)
    {
        var newPayment = new Payment(description, this, nominalValue, dueDay);
        Payments.Add(newPayment);

        return newPayment;
    }

    public Payment? GetPayment(Guid paymentId)
    {
        return Payments.FirstOrDefault(x => x.Id == paymentId);
    }

    public bool RegisterExecutedPayment(Payment payment, ExecutedPayment executedPayment)
    {
        var selectedPmt = GetPayment(payment.Id);
        if (selectedPmt == null)
        {
            return false;
        }

        selectedPmt!.AddExecutedPayment(executedPayment);

        return true;
    }
}
