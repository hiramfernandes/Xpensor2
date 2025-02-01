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

        [HttpPost("monthly-report")]
        public async Task<IActionResult> AddExpenditures(DateTime referenceDate)
        {
            var user = new User("Hiram");
            var report = await _paymentService.GenerateMonthlyReport(user, referenceDate);

            return Ok(report);
        }
    }
}
