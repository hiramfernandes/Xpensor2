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
        Task<IEnumerable<ExpenseDto>> GetExpendituresForPeriod(GetMonthlyReportRequest request);
        Task AddExpenditures(IEnumerable<Expense> expenditures);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IExpensesRepository _paymentRepository;

        public PaymentService(IExpensesRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpendituresForPeriod(GetMonthlyReportRequest request)
        {
            var result = await _paymentRepository.GetExpendituresAsync(request.ReportMonth, request.ReportYear);
            return result.Select(x => new ExpenseDto()
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

        public Task<IEnumerable<Expense>> GenerateMonthlyReport(GenerateMonthlyReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}