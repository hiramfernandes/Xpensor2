using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        // Payments
        Task<Payment> AddInstallmentPayment(User user, string description, decimal installmentValue, int numberOfInstallments, int dueDay, DateTime startDate);
        Task<Payment> AddRecurringPayment(User user, string description, decimal amount, int dueDay);
        Task<Payment> AddSinglePayment(User user, string description, decimal nominalValue, DateTime dueDate);


        // Expenditures
        Task<IEnumerable<Expenditure>> GenerateMonthlyReport(User owner, DateTime referenceDate);
        Task AddExpenditures(IEnumerable<Expenditure> expenditures);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> AddRecurringPayment(User user, string description, decimal amount, int dueDay)
        {
            var newPayment = user.CreateRecurringPayment(description, amount, dueDay);
            await _paymentRepository.AddPayment(newPayment);

            return newPayment;
        }

        public async Task<Payment> AddInstallmentPayment(User user,
                                             string description,
                                             decimal installmentValue,
                                             int numberOfInstallments,
                                             int dueDay,
                                             DateTime startDate)
        {
            var installment = user.CreateInstallment(description, installmentValue, numberOfInstallments, dueDay, startDate);
            await _paymentRepository.AddPayment(installment);

            return installment;
        }

        public async Task<Payment> AddSinglePayment(User user,
                                                    string description,
                                                    decimal nominalValue,
                                                    DateTime dueDate)
        {
            var single = user.CreateSinglePayment(description, user, nominalValue, dueDate);
            await _paymentRepository.AddPayment(single);

            return single;
        }

        public async Task<IEnumerable<Expenditure>> GenerateMonthlyReport(User owner, DateTime referenceDate)
        {
            // From Payments you can get to Expenditures in three steps:
            /// 1) Recurring payments - every iteration generates them as long as they are active/enabled
            /// 2) Installments - a bit more tricky, but based on the first installment count to the current month to see if there are pending ones
            /// 3) Single - just check if the due date belongs to the current exercise

            // Payments that haven't been paid and are due during the reference period
            // Need to check:
            // 3) If there's some payment left behind

            var recurring = _paymentRepository.GetRecurringPayments(referenceDate);
            var single = _paymentRepository.GetSinglePayments(referenceDate);
            var installments = _paymentRepository.GetInstallments(referenceDate);

            var monthlyExpenses =
                recurring.Concat(single)
                         .Concat(installments)
                         .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year))
                         .ToList();

            await _paymentRepository.AddExpendituresRange(monthlyExpenses);

            return monthlyExpenses;
        }
        
        public async Task AddExpenditures(IEnumerable<Expenditure> expenditures)
        {
            await _paymentRepository.AddExpendituresRange(expenditures);
        }

        private static Expenditure MapFrom(Payment payment, int month, int year)
        {
            var dueDate = new DateTime(year, month, payment.DueDay > 0 ? payment.DueDay : 1);
            return new Expenditure(payment, dueDate, payment.Description, string.Empty);
        }
    }
}
