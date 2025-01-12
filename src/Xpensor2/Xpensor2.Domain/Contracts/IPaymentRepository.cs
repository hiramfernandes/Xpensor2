using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Contracts;

public interface IPaymentRepository
{
    IEnumerable<Payment> GetRecurringPayments(DateTime referenceDate);
    IEnumerable<Payment> GetInstallments(DateTime referenceDate);
    IEnumerable<Payment> GetSinglePayments(DateTime referenceDate);
    Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses);
}
