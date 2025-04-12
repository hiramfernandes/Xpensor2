using Xpensor2.Application.Requests;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        // Payments
        Task<Payment> AddInstallmentPayment(CreateInstallmentPaymentRequest request);
        Task<Payment> AddRecurringPayment(CreateRecurringPaymentRequest request);
        Task<Payment> AddSinglePayment(CreateSinglePaymentRequest request);

        // Expenditures
        Task<IEnumerable<Expenditure>> GenerateMonthlyReport(GenerateMonthlyReportRequest request);
        Task AddExpenditures(IEnumerable<Expenditure> expenditures);

        // Executed Payments
        Task ExecutePayment(ExecutePaymentRequest request);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> AddRecurringPayment(CreateRecurringPaymentRequest request)
        {
            // TODO: Replace with a userId fetch from DB
            var user = new User(request.UserName);

            var newPayment = user.CreateRecurringPayment(request.PaymentDescription, request.NominalValue, request.DueDay);
            await _paymentRepository.AddPayment(newPayment);

            return newPayment;
        }

        public async Task<Payment> AddInstallmentPayment(CreateInstallmentPaymentRequest request)
        {
            // TODO: Replace with a userId fetch from DB
            var user = new User(request.UserName);

            var installment = user.CreateInstallment(
                request.PaymentDescription,
                request.InstallmentValue,
                request.NumberOfInstallments,
                request.StartDate.Day,
                request.StartDate.ToDateTime(TimeOnly.MinValue));

            await _paymentRepository.AddPayment(installment);

            return installment;
        }

        public async Task<Payment> AddSinglePayment(CreateSinglePaymentRequest request)
        {
            // TODO: Replace with a userId fetch from DB
            var user = new User(request.UserName);

            var single = user.CreateSinglePayment(request.Description, user, request.NominalValue, request.DueDate.ToDateTime(TimeOnly.MinValue));
            await _paymentRepository.AddPayment(single);

            return single;
        }

        public async Task<IEnumerable<Expenditure>> GenerateMonthlyReport(GenerateMonthlyReportRequest request)
        {
            // From Payments you can get to Expenditures in three steps:
            /// 1) Recurring payments - every iteration generates them as long as they are active/enabled
            /// 2) Installments - a bit more tricky, but based on the first installment count to the current month to see if there are pending ones
            /// 3) Single - just check if the due date belongs to the current exercise

            // Payments that haven't been paid and are due during the reference period
            // Need to check:
            // 3) If there's some payment left behind

            // TODO: Replace with a userId fetch from DB
            var user = new User(request.UserName);
            var referenceDate = new DateTime(request.ReportYear, request.ReportMonth, 1);

            var recurring = _paymentRepository.GetRecurringPayments(referenceDate);
            var single = _paymentRepository.GetSinglePayments(referenceDate);

            var installments = await _paymentRepository.GetInstallments(referenceDate);

            var monthlyExpenses =
                recurring.Concat(single)
                         .Concat(installments)
                         .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year))
                         .ToList();

            // Disabling persistence in order to make some more tests
            // TODO: Re-enable once done validating
            //await _paymentRepository.AddExpendituresRange(monthlyExpenses);

            return monthlyExpenses;
        }

        public async Task AddExpenditures(IEnumerable<Expenditure> expenditures)
        {
            await _paymentRepository.AddExpendituresRange(expenditures);
        }

        private static Expenditure MapFrom(Payment payment, int month, int year)
        {
            var dueDate = new DateTime(year, month, payment.DueDay);
            return new Expenditure(payment, dueDate, payment.Description, string.Empty);
        }

        public Task ExecutePayment(ExecutePaymentRequest request)
        {
            //var expenditure = _paymentRepository.GetExpenditure(request.ExpenditureId);

            throw new NotImplementedException();
        }
    }
}
