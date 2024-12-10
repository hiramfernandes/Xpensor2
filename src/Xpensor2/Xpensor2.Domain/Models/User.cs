namespace Xpensor2.Domain.Models;

public class User
{
    public User(string? name)
    {
        Id = Guid.NewGuid();
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

    public IEnumerable<Payment> CreateInstallments(string description,
                                   decimal installmentValue,
                                   long numberOfInstallments,
                                   int dueDay,
                                   DateTime startDate)
    {
        var generatedPayments = new List<Payment>();

        for (int i = 1; i <= numberOfInstallments; i++)
        {
            var installmentPayment = new Payment(
                description: description,
                owner: this,
                nominalValue: installmentValue,
                totalInstallments: numberOfInstallments,
                installmentNumber: i,
                dueDay: dueDay,
                startDate: startDate);

            installmentPayment.DueDate = startDate.AddMonths(i - 1);

            Payments.Add(installmentPayment);
            generatedPayments.Add(installmentPayment);
        }

        return generatedPayments;
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
