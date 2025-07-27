using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application;

public interface IExpensesService
{
    Task<IEnumerable<ExpenditureDto>> GetExpendituresForPeriod(int month, int year, string userId);
    Task CreateExpenseAsync(CreateExpenseRequest request);
}

public class ExpensesService : IExpensesService
{
    private readonly IExpensesRepository _expensesRepository;

    public ExpensesService(IExpensesRepository expensesRepository)
    {
        _expensesRepository = expensesRepository;
    }

    public async Task CreateExpenseAsync(CreateExpenseRequest request)
    {
        if (request == null)
            throw new InvalidOperationException("Request cannot be null");

        var expense = new Expense(request.DueDate, request.ExpenseValue, request.Description, request.SpecialInstruction);

        await _expensesRepository.AddExpenseAsync(expense);
    }

    public async Task<IEnumerable<ExpenditureDto>> GetExpendituresForPeriod(int month, int year, string userId)
    {
        var result = await _expensesRepository.GetExpendituresAsync(month, year);
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
}
