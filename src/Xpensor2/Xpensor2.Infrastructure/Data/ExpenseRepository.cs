using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Infrastructure.Data.Settings;

namespace Xpensor2.Infrastructure.Data;

public class PaymentRepository : IExpenseRepository
{
    IMongoCollection<Expenditure> _expenditures;

    public PaymentRepository(IOptions<PaymentsDatabaseSettings> databaseSettings)
    {
        var connectionString = databaseSettings.Value.ConnectionString;
        var collectionName = databaseSettings.Value.CollectionName;
        var dbName = databaseSettings.Value.DatabaseName;

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _expenditures = mongoDatabase.GetCollection<Expenditure>("expenditures");
    }

    public async Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses)
    {
        await _expenditures.InsertManyAsync(monthlyExpenses);
    }

    private bool BelongsToTheInstallmentRange(DateTime referenceDate, DateTime installmentStartDate, int numberOfInstallments)
    {
        // Get first day of month for both dates
        var firstDayOfTheMonthReferenceDate = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var firstDayOfTheMonthInstallmentStartDate = new DateTime(installmentStartDate.Year, installmentStartDate.Month, 1);

        // Before the beginning of the installment period
        if (firstDayOfTheMonthReferenceDate >= firstDayOfTheMonthInstallmentStartDate &&
            firstDayOfTheMonthReferenceDate <= firstDayOfTheMonthInstallmentStartDate.AddMonths(numberOfInstallments - 1))
            return true;

        return false;
    }

    public async Task<Expenditure> GetExpenditureAsync(string id)
    {
        var expenditure = _expenditures.Find(x => x.Id == id);

        return await expenditure.FirstAsync();
    }

    public async Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment)
    {
        var filter = Builders<Expenditure>.Filter.Eq("Id", expenditureId);
        var update = Builders<Expenditure>.Update.Set("ExecutedPayment", executedPayment);

        await _expenditures.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<Expenditure>> GetExpendituresAsync(int month, int year)
    {
        var lowerLimit = new DateTime(year, month, 1);
        var upperLimit = lowerLimit.AddMonths(1);

        var filter = Builders<Expenditure>.Filter.Where(x => x.DueDate >= lowerLimit && x.DueDate < upperLimit);
        var result = await _expenditures.FindAsync(filter);

        return result.ToEnumerable();
    }
}
