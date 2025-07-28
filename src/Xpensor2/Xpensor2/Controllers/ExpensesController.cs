using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;
using Xpensor2.Application.Services;

namespace Xpensor2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpGet("get-monthly-report")]
        [ProducesResponseType<IEnumerable<ExpenseDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpenses([FromQuery] int month, [FromQuery] int year)
        {
            var results = await _expensesService.GetExpendituresForPeriod(month, year, "Hiram");

            return Ok(results);
        }

        [HttpPost("add-expense")]
        public async Task<IActionResult> AddExpense([FromBody] CreateExpenseRequest request)
        {
            await _expensesService.CreateExpenseAsync(request);

            return Created();
        }

        [HttpPost("pay")]
        public async Task<IActionResult> UpdateExpense([FromBody] ExecutePaymentRequest request)
        {
            await _expensesService.ExecutePayment(request);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseRequest request)
        {
            await _expensesService.UpdateExpense(request);

            return Ok();
        }
    }
}
