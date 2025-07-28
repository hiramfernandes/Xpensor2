using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;
using Xpensor2.Domain.Contracts;
using Xpensor2.Domain.Models;

namespace Xpensor2.Application.Services;

public interface IExpensesService
{
    Task<IEnumerable<ExpenseDto>> GetExpendituresForPeriod(int month, int year, string userId);
    Task CreateExpenseAsync(CreateExpenseRequest request);

    // Payments
    Task ExecutePayment(ExecutePaymentRequest request);
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

    public async Task<IEnumerable<ExpenseDto>> GetExpendituresForPeriod(int month, int year, string userId)
    {
        var result = await _expensesRepository.GetExpendituresAsync(month, year);
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

    public async Task ExecutePayment(ExecutePaymentRequest request)
    {
        var expense = await _expensesRepository.GetExpenditureAsync(request.ExpenseId);
        if (expense == null)
        {
            throw new InvalidOperationException($"Expense not found (id: {request.ExpenseId})");
        }

        var executedPayment = new ExecutedPayment(request.PaymentMethod, request.PaidValue, request.PaidDate, new User(request.PaidBy));

        await _expensesRepository.UpdateExpenditurePayment(expense.Id, executedPayment);
    }
}
