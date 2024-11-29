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
            var pmtDescription = "CEEE";
            var pmt = user.CreatePayment(
                description: pmtDescription, 
                nominalValue: 150, 
                dueDay: 5);

            var slice = new PaymentSlice();
            slice.AddPayment(pmt);
            var referenceDate = DateTime.Today;
            pmt.DueDate = referenceDate;
            var expenditures = slice.GenerateExpenditures(referenceDate);

            expenditures.Should().NotBeNull();
            expenditures.Should().NotBeEmpty();
            expenditures!.First().Name.Should().Be(pmtDescription);
        }
    }
}
