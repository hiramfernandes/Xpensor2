using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Test.Helpers
{

    public class InMemoryExpenditureRepository : IExpensesRepository
    {
        private readonly User _user;

        public InMemoryExpenditureRepository(User user)
        {
            _user = user;
        }

        public async Task AddExpensesRange(IEnumerable<Expense> monthlyExpenses)
        {
            await Task.Delay(10);
            foreach (var expense in monthlyExpenses)
            {
                _user.Expenses.Add(expense);
            }
        }

        public Task<Expense> GetExpenditureAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GetExpendituresAsync(int month, int year)
        {
            throw new NotImplementedException();
        }
    }

}
