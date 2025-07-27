using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        // Expenditures
        Task<IEnumerable<Expense>> GenerateMonthlyReport(GenerateMonthlyReportRequest request);
        Task<IEnumerable<ExpenditureDto>> GetExpendituresForPeriod(GetMonthlyReportRequest request);
        Task AddExpenditures(IEnumerable<Expense> expenditures);

        // Executed Payments
        Task ExecutePayment(ExecutePaymentRequest request);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IExpensesRepository _paymentRepository;

        public PaymentService(IExpensesRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<ExpenditureDto>> GetExpendituresForPeriod(GetMonthlyReportRequest request)
        {
            var result = await _paymentRepository.GetExpendituresAsync(request.ReportMonth, request.ReportYear);
            return result.Select(x => new ExpenditureDto()
            {
                Id = x.Id,
                ExpenseName = x.Name,
                DueDate = x.DueDate,
                Value = x.Value,
                GeneralInfo = x.GeneralInfo,
                Paid = x.ExecutedPayment != null,
                PaymentDate = x.ExecutedPayment?.PaidDate
            });
        }

        public async Task AddExpenditures(IEnumerable<Expense> expenditures)
        {
            await _paymentRepository.AddExpensesRange(expenditures);
        }

        public async Task ExecutePayment(ExecutePaymentRequest request)
        {
            var expenditure = await _paymentRepository.GetExpenditureAsync(request.ExpenditureId);
            if (expenditure == null)
            {
                throw new InvalidOperationException($"Expenditure not found (id: {request.ExpenditureId})");
            }

            var executedPayment = new ExecutedPayment(request.PaymentMethod, request.PaidValue, request.PaidDate, new User(request.PaidBy));

            await _paymentRepository.UpdateExpenditurePayment(expenditure.Id, executedPayment);
        }

        public Task<IEnumerable<Expense>> GenerateMonthlyReport(GenerateMonthlyReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}