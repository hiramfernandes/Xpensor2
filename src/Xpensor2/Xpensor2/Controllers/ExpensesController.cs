using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application;
using Xpensor2.Application.Requests;

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
        [ProducesResponseType<IEnumerable<Object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpenses([FromQuery] int month, [FromQuery] int year)
        {
            var results = await _expensesService.GetExpendituresForPeriod(month, year, "Hiram");

            return Ok(results);
        }

        [HttpPost("AddExpense")]
        public async Task AddExpense(CreateExpenseRequest request)
        {
            await _expensesService.CreateExpenseAsync(request);

            Created();
        }
    }
}
