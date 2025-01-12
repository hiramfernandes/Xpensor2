using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Contracts
{
    public interface IExpenditureRepository
    {
        IEnumerable<Payment> GetRecurringPayments(DateTime referenceDate);
        IEnumerable<Payment> GetInstallments(DateTime referenceDate);
        IEnumerable<Payment> GetSinglePayments(DateTime referenceDate);
        Task AddRange(IEnumerable<Expenditure> monthlyExpenses);
    }
}
