using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.AddPayment
{
    public interface IPaymentService
    {
        // Expenditures
        Task<IEnumerable<Expenditure>> GenerateMonthlyReport(GenerateMonthlyReportRequest request);
        Task<IEnumerable<ExpenditureDto>> GetExpendituresForPeriod(GetMonthlyReportRequest request);
        Task AddExpenditures(IEnumerable<Expenditure> expenditures);

        // Executed Payments
        Task ExecutePayment(ExecutePaymentRequest request);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IExpenseRepository _paymentRepository;

        public PaymentService(IExpenseRepository paymentRepository)
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

        public async Task AddExpenditures(IEnumerable<Expenditure> expenditures)
        {
            await _paymentRepository.AddExpendituresRange(expenditures);
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

        public Task<IEnumerable<Expenditure>> GenerateMonthlyReport(GenerateMonthlyReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}