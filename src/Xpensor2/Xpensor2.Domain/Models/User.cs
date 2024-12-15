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
    public List<Expenditure> Expenditures { get; private set; } = [];

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
        var installmentPayment = new Payment(
            description: description,
            owner: this,
            nominalValue: installmentValue,
            dueDay: dueDay,
            numberOfInstallments: numberOfInstallments,
            startDate: startDate);

        Payments.Add(installmentPayment);
        return installmentPayment;
    }

    public Payment? GetPayment(Guid paymentId)
    {
        return Payments.FirstOrDefault(x => x.Id == paymentId);
    }

    public Expenditure? GetExpenditure(Guid id)
    {
        return Expenditures.FirstOrDefault(x => x.Id == id);
    }

    public bool RegisterExecutedPayment(Expenditure expenditure, ExecutedPayment executedPayment)
    {
        var selectedExpenditure = GetExpenditure(expenditure.Id);
        if (selectedExpenditure == null)
        {
            return false;
        }

        selectedExpenditure!.Pay(executedPayment);

        return true;
    }
}
