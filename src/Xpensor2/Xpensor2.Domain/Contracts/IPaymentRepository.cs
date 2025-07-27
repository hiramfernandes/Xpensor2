using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Contracts;

public interface IExpenseRepository
{
    Task<Expenditure> GetExpenditureAsync(string id);
    Task<IEnumerable<Expenditure>> GetExpendituresAsync(int month, int year);
    Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses);
    Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment);
}
