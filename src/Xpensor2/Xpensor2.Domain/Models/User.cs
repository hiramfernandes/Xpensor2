namespace Xpensor2.Domain.Models;

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

    public Payment CreateInstallment(string description,
                                     decimal installmentValue,
                                     int numberOfInstallments,
                                     int dueDay,
                                     DateTime startDate)
    {
        var installment = new Payment(description, this, installmentValue, dueDay, numberOfInstallments, startDate);
        Payments.Add(installment);

        return installment;
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

        selectedPmt!.ExecutePayment(executedPayment);

        return true;
    }
}
