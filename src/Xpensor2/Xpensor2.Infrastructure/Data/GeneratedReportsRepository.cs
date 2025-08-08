using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xpensor2.Domain.Models;
using Xpensor2.Infrastructure.Data.Settings;

namespace Xpensor2.Infrastructure.Data;

public class GeneratedReportsRepository
{
    IMongoCollection<GeneratedReport> _reports;

    public GeneratedReportsRepository(IOptions<PaymentsDatabaseSettings> databaseSettings)
    {
        var connectionString = databaseSettings.Value.ConnectionString;
        var collectionName = "expenses";
        var dbName = databaseSettings.Value.DatabaseName;

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _reports = mongoDatabase.GetCollection<GeneratedReport>(collectionName);

    }

    public async Task AddReportAsync(GeneratedReport report)
    {
        await _reports.InsertOneAsync(report);
    }
}
