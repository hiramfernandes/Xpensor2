using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Models;

public class PaymentSlice
{
    private readonly Contracts.IPaymentRepository _paymentRepository;

    public PaymentSlice(User owner, Contracts.IPaymentRepository paymentRepository)
    {
        Owner = owner;
        _paymentRepository = paymentRepository;
    }

    public User Owner { get; set; }

    public IEnumerable<Expenditure>? MonthlyReport(DateTime referenceDate)
    {
        // From Payments you can get to Expenditures in three steps:
        /// 1) Recurring payments - every iteration generates them as long as they are active/enabled
        /// 2) Installments - a bit more tricky, but based on the first installment count to the current month to see if there are pending ones
        /// 3) Single - just check if the due date belongs to the current exercise

        // Payments that haven't been paid and are due during the reference period
        // Need to check:
        // 3) If there's some payment left behind

        // Recurring
        var recurring = _paymentRepository.GetRecurringPayments(referenceDate);
        var single = _paymentRepository.GetSinglePayments(referenceDate);
        var installments = _paymentRepository.GetInstallments(referenceDate);

        var monthlyExpenses = 
            recurring.Concat(single)
                     .Concat(installments)
                     .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year))
                     .ToList();

        _paymentRepository.AddExpendituresRange(monthlyExpenses);

        return monthlyExpenses;
    }

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        var dueDate = new DateTime(year, month, payment.DueDay);
        return new Expenditure(payment, dueDate, payment.Description, string.Empty);
    }
}

public class InMemoryExpenditureRepository : IPaymentRepository
{
    private readonly User _user;

    public InMemoryExpenditureRepository(User user)
    {
        _user = user;
    }

    public IEnumerable<Payment> GetRecurringPayments(DateTime referenceDate)
    {
        return _user.Payments
                        .Where(x => x.PaymentType == PaymentType.Recurring);
    }

    public IEnumerable<Payment> GetSinglePayments(DateTime referenceDate)
    {
        return _user.Payments
                        .Where(x => x.PaymentType == PaymentType.Single)
                        .Where(x => x.DueDate.Month == referenceDate.Month && x.DueDate.Year == referenceDate.Year);
    }

    public async Task AddExpendituresRange(IEnumerable<Expenditure> monthlyExpenses)
    {
        await Task.Delay(10);
        _user.Expenditures.AddRange(monthlyExpenses);
    }

    public IEnumerable<Payment> GetInstallments(DateTime referenceDate)
    {
        return _user.Payments
                    .Where(x => x.PaymentType == PaymentType.Installment)
                    .Where(x => x.StartDate.HasValue && x.NumberOfInstallments.HasValue)
                    .Where(x => BelongsToTheInstallmentRange(referenceDate, x.StartDate!.Value, x.NumberOfInstallments!.Value))
                    .Where(x => !_user.Expenditures.Any(y => y.Payment.Id == x.Id && y.ExecutedPayment != null));
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
}
