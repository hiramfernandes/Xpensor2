namespace Xpensor2.Domain.Models;

public class User
{
    public User(string? name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public string Id { get; init; }
    public string? Name { get; init; }
    public IList<Payment> Payments { get; private set; } = [];
    public List<Expenditure> Expenditures { get; private set; } = [];

    public Payment CreateRecurringPayment(string description, decimal nominalValue, int dueDay)
    {
        return new Payment(description, this, nominalValue, dueDay);
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

        return installmentPayment;
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
