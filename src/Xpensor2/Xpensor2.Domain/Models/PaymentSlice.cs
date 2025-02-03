using Xpensor2.Domain.Contracts;

namespace Xpensor2.Domain.Models;

// TODO: Remove this class and refactor tests to remove all references
public class PaymentSlice
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentSlice(User owner, IPaymentRepository paymentRepository)
    {
        Owner = owner;
        _paymentRepository = paymentRepository;
    }

    public User Owner { get; set; }

    public async Task<IEnumerable<Expenditure>>? MonthlyReport(DateTime referenceDate)
    {
        // From Payments you can get to Expenditures in three steps:
        /// 1) Recurring payments - every iteration generates them as long as they are active/enabled
        /// 2) Installments - a bit more tricky, but based on the first installment count to the current month to see if there are pending ones
        /// 3) Single - just check if the due date belongs to the current exercise

        // Payments that haven't been paid and are due during the reference period
        // Need to check:
        // 3) If there's some payment left behind

        // TODO: Move this to repo
        var recurring = _paymentRepository.GetRecurringPayments(referenceDate);
        var single = _paymentRepository.GetSinglePayments(referenceDate);
        var installments = await _paymentRepository.GetInstallments(referenceDate);

        var monthlyExpenses = 
            recurring.Concat(single)
                     .Concat(installments)
                     .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year))
                     .ToList();

        await _paymentRepository.AddExpendituresRange(monthlyExpenses);

        return monthlyExpenses;
    }

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        var dueDate = new DateTime(year, month, payment.DueDay);
        return new Expenditure(payment, dueDate, payment.Description, string.Empty);
    }
}
