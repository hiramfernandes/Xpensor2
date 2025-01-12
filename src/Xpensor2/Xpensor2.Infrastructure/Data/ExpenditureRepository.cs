using MongoDB.Driver;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Infrastructure.Data
{
    public class ExpenditureRepository : IExpenditureRepository
    {
        IMongoCollection<Expenditure> _expenditures;

        public ExpenditureRepository(IMongoCollection<Expenditure> expenditures)
        {
            _expenditures = expenditures;
        }

        public async Task Add(Expenditure newExpenditure)
        {
            await _expenditures.InsertOneAsync(newExpenditure);
        }

        public async Task AddRange(IEnumerable<Expenditure> monthlyExpenses)
        {
            await _expenditures.InsertManyAsync(monthlyExpenses);
        }

        public IEnumerable<Payment> GetInstallments(DateTime referenceDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Payment> GetRecurringPayments(DateTime referenceDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Payment> GetSinglePayments(DateTime referenceDate)
        {
            throw new NotImplementedException();
        }
    }
}
