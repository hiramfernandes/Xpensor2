using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Infrastructure.Data.Settings;

namespace Xpensor2.Infrastructure.Data;

public class PaymentRepository : IPaymentRepository
{
    IMongoCollection<Expenditure> _expenditures;
    IMongoCollection<Payment> _payments;

    public PaymentRepository(IOptions<PaymentsDatabaseSettings> databaseSettings)
    {
        var connectionString = databaseSettings.Value.ConnectionString;
        var collectionName = databaseSettings.Value.CollectionName;
        var dbName = databaseSettings.Value.DatabaseName;

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _expenditures = mongoDatabase.GetCollection<Expenditure>("expenditure");
        _payments = mongoDatabase.GetCollection<Payment>("payments");
    }

    public async Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses)
    {
        await _expenditures.InsertManyAsync(monthlyExpenses);
    }

    public Task AddPayment(Payment payment)
    {
        return _payments.InsertOneAsync(payment);
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
