using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application.AddPayment;
using Xpensor2.Application.Requests;
using Xpensor2.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpPost("recurring")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddRecurring([FromBody] CreateRecurringPaymentRequest request)
        {
            if (request == null)
                return BadRequest(nameof(request));

            var pmt = await _paymentService.AddRecurringPayment(request);

            return Ok(pmt);
        }

        // POST api/<PaymentsController>
        [HttpPost("installment")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInstallment([FromBody] CreateInstallmentPaymentRequest request)
        {
            if (request == null)
                return BadRequest(nameof(request));

            var pmt = await _paymentService.AddInstallmentPayment(request);

            return Ok(pmt);
        }

        [HttpPost("single")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSingle([FromBody] CreateSinglePaymentRequest request)
        {
            var pmt = await _paymentService.AddSinglePayment(request);

            return Ok(pmt);
        }

        [HttpPost("register-payment")]
        public async Task<IActionResult> RegisterPayment(ExecutePaymentRequest executePaymentRequest)
        {
            try
            {
                await _paymentService.ExecutePayment(executePaymentRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // TODO: Change to Get
        [HttpPost("create-monthly-report")]
        [ProducesResponseType<IEnumerable<Expenditure>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateMonthlyExpenditures(GenerateMonthlyReportRequest request)
        {
            if (request == null)
                return BadRequest(nameof(request));

            var expenditures = await _paymentService.GenerateMonthlyReport(request);

            if (request.Persist == true)
                await _paymentService.AddExpenditures(expenditures);

            return Ok(expenditures);
        }

        [HttpGet("get-monthly-report")]
        public async Task GetMonthlyExpenditures([FromQuery]int month, [FromQuery]int year)
        {
            var request = new GetMonthlyReportRequest()
            {
                UserName = "Hiram",
                ReportMonth = month,
                ReportYear = year
            };

            var expenditures = await _paymentService.GetExpendituresForPeriod(request);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayExpenditure(ExecutePaymentRequest request)
        {
            await _paymentService.ExecutePayment(request);
            return Ok();
        }
    }
}
