using MongoDB.Driver;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Infrastructure.Data;

public class PaymentRepository : IPaymentRepository
{
    IMongoCollection<Expenditure> _expenditures;

    public PaymentRepository(IMongoCollection<Expenditure> expenditures)
    {
        _expenditures = expenditures;
    }

    public async Task Add(Expenditure newExpenditure)
    {
        await _expenditures.InsertOneAsync(newExpenditure);
    }

    public async Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses)
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
