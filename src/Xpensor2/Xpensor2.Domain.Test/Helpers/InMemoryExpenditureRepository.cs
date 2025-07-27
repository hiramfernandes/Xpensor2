using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Test.Helpers
{

    public class InMemoryExpenditureRepository : IExpenseRepository
    {
        private readonly User _user;

        public InMemoryExpenditureRepository(User user)
        {
            _user = user;
        }

        public async Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses)
        {
            await Task.Delay(10);
            foreach (var expense in monthlyExpenses)
            {
                _user.Expenditures.Add(expense);
            }
        }

        public Task<Expenditure> GetExpenditureAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expenditure>> GetExpendituresAsync(int month, int year)
        {
            throw new NotImplementedException();
        }
    }

}
