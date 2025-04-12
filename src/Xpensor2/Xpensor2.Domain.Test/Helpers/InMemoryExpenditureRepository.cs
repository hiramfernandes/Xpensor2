using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;
using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Test.Helpers
{

    public class InMemoryExpenditureRepository : IPaymentRepository
    {
        private readonly User _user;

        public InMemoryExpenditureRepository(User user)
        {
            _user = user;
        }

        public async Task AddPayment(Payment payment)
        {
            await Task.Delay(200);
            _user.Payments.Add(payment);
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
            foreach (var expense in monthlyExpenses)
            {
                _user.Expenditures.Add(expense);
            }
        }

        public async Task<IEnumerable<Payment>> GetInstallments(DateTime referenceDate)
        {
            await Task.Delay(100);

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

}
