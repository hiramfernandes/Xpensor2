using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application.AddPayment;
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

        // GET: api/<PaymentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PaymentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PaymentsController>
        [HttpPost("recurring")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddRecurring([FromBody] string paymentDescription)
        {
            var pmt = await _paymentService.AddRecurringPayment(new User("Hiram"), paymentDescription, 100, 5);

            return Ok(pmt);
        }

        // POST api/<PaymentsController>
        [HttpPost("installment")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInstallment([FromBody] string paymentDescription)
        {
            var pmt = await _paymentService.AddInstallmentPayment(new User("Hiram"), paymentDescription, 100, 3, 5, DateTime.Today);

            return Ok(pmt);
        }

        [HttpPost("single")]
        [ProducesResponseType<Payment>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSingle([FromBody] string paymentDescription)
        {
            var pmt = await _paymentService.AddSinglePayment(new User("Hiram"), paymentDescription, 100, DateTime.Today.AddDays(12));

            return Ok(pmt);
        }

        [HttpPost("expenditures")]
        public async Task<IActionResult> AddExpenditures([FromBody] string[] expenditures)
        {
            foreach (var expenditureDescription in expenditures)
            {
                var user = new User("Hiram");
                // Create single pmt
                var pmt = await _paymentService.AddSinglePayment(user, "A gift", 50, DateTime.Today);
                //var expenditure = new 

                
                //var expenditure = 
                //await _paymentService.AddExpenditures(new User("Hiram"), paymentDescription, 100, DateTime.Today.AddDays(12));
            }

            return Ok();
        }
    }
}
