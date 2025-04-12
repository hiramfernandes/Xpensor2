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
    public IList<Expenditure> Expenditures { get; private set; } = [];

    #region Payment Operations
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

    public Payment CreateSinglePayment(string description,
                                       User owner,
                                       decimal nominalValue,
                                       DateTime dueDate)
    {
        return new Payment(description, owner, nominalValue, dueDate);
    }
    #endregion Payment Operations

    public Expenditure? GetExpenditure(string id)
    {
        return Expenditures.FirstOrDefault(x => x.Id == id);
    }

    // TODO: Move away from this concept
    // Instead, make the user perform the payment execution which will lead to the actual operation underneath, which consists of:
    // Retrieving the existing payment (and throw an exception if it doesn't exist)
    // Assign an executed payment to it
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
