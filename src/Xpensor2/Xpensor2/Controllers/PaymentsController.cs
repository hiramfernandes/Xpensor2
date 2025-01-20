using Microsoft.AspNetCore.Mvc;
using Xpensor2.Application.AddPayment;

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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string paymentDescription)
        {
            var pmt = await _paymentService.AddRecurringPayment(new Domain.Models.User("Hiram"), paymentDescription, 100, 5);

            return Ok(pmt);
        }

        // PUT api/<PaymentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaymentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
