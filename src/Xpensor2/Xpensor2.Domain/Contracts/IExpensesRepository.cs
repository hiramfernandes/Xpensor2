using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Contracts;

public interface IExpensesRepository
{
    Task<Expense> GetExpenditureAsync(string id);
    Task<IEnumerable<Expense>> GetExpendituresAsync(int month, int year);
    Task AddExpensesRange(IEnumerable<Expense> monthlyExpenses);
    Task AddExpenseAsync(Expense expense);
    Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment);
    Task UpdateExpenseAsync(string expenseId, Expense updatedExpense);
}
