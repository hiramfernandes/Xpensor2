using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Infrastructure.Data.Settings;

namespace Xpensor2.Infrastructure.Data;

public class ExpensesRepository : IExpensesRepository
{
    IMongoCollection<Expense> _expenses;

    public ExpensesRepository(IOptions<PaymentsDatabaseSettings> databaseSettings)
    {
        var connectionString = databaseSettings.Value.ConnectionString;
        // var collectionName = databaseSettings.Value.CollectionName;
        var dbName = databaseSettings.Value.DatabaseName;

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _expenses = mongoDatabase.GetCollection<Expense>("expenses");
    }

    public async Task AddExpenseAsync(Expense expense)
    {
        await _expenses.InsertOneAsync(expense);
    }

    public async Task AddExpensesRange(IEnumerable<Expense> monthlyExpenses)
    {
        await _expenses.InsertManyAsync(monthlyExpenses);
    }

    public async Task<Expense> GetExpenditureAsync(string id)
    {
        var expenditure = _expenses.Find(x => x.Id == id);

        return await expenditure.FirstAsync();
    }

    public async Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment)
    {
        var filter = Builders<Expense>.Filter.Eq("Id", expenditureId);
        var update = Builders<Expense>.Update.Set("ExecutedPayment", executedPayment);

        await _expenses.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<Expense>> GetExpendituresAsync(int month, int year)
    {
        var lowerLimit = new DateTime(year, month, 1);
        var upperLimit = lowerLimit.AddMonths(1);

        var filter = Builders<Expense>.Filter.Where(x => x.DueDate >= lowerLimit.Date && x.DueDate < upperLimit);
        var result = await _expenses.FindAsync(filter);

        return result.ToEnumerable();
    }
}
