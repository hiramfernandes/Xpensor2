using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application.AddPayment;
using Xpensor2.Application.Requests;
using Xpensor2.Application.Responses;

namespace Xpensor2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-monthly-report")]
        [ProducesResponseType<IEnumerable<ExpenditureDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyExpenditures([FromQuery] int month, [FromQuery] int year)
        {
            var request = new GetMonthlyReportRequest()
            {
                UserName = "Hiram",
                ReportMonth = month,
                ReportYear = year
            };

            var expenditures = await _paymentService.GetExpendituresForPeriod(request);

            return Ok(expenditures);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayExpenditure(ExecutePaymentRequest request)
        {
            await _paymentService.ExecutePayment(request);
            return Ok();
        }
    }
}
