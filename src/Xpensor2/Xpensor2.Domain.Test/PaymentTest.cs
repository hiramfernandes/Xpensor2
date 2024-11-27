using FluentAssertions;
using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Test
{
    public class PaymentTest
    {
        [Fact]
        public void GenerateExpenses_ShouldCreateExpenses_BasedOnExistingPayments()
        {
            var user = new User();
            var pmt = user.CreatePayment("test", 123);

            var slice = new PaymentSlice();
            slice.AddPayment(pmt);
            var referenceDate = DateTime.Today;
            pmt.DueDate = referenceDate;
            var expenditures = slice.GenerateExpenditures(referenceDate, referenceDate.AddDays(1));

            expenditures.Should().NotBeNull();
            expenditures.Should().NotBeEmpty();
        }
    }
}
