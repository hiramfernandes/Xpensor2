using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Contracts;

public interface IPaymentRepository
{
    Task AddPayment(Payment payment);
    IEnumerable<Payment> GetRecurringPayments(DateTime referenceDate);
    Task<IEnumerable<Payment>> GetInstallments(DateTime referenceDate);
    IEnumerable<Payment> GetSinglePayments(DateTime referenceDate);
    Task<Expenditure> GetExpenditureAsync(string id);
    Task<IEnumerable<Expenditure>> GetExpendituresAsync(int month, int year);
    Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses);
    Task UpdateExpenditurePayment(string expenditureId, ExecutedPayment executedPayment);
}
