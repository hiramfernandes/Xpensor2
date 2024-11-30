using FluentAssertions;
using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Test;

public class PaymentTest
{
    [Fact]
    public void GenerateExpenses_ShouldCreateExpenses_BasedOnExistingPayments()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "name");
        var pmtDescription = "CEEE";
        var dueDay = 5;
        var pmt = user.CreatePayment(description: pmtDescription, nominalValue: 150, dueDay: dueDay);
        var pmt2 = user.CreatePayment(description: "Pmt2", nominalValue: 100, dueDay: 2);
        var pmt3 = user.CreatePayment(description: "Pmt3", nominalValue: 200, dueDay: 5);
        var pmt4 = user.CreatePayment(description: "Pmt4", nominalValue: 300, dueDay: 10);

        var slice = new PaymentSlice();
        slice.AddPayment(pmt);
        slice.AddPayment(pmt2);
        slice.AddPayment(pmt3);
        slice.AddPayment(pmt4);
        var referenceDate = DateTime.Today;
        pmt.DueDate = referenceDate;

        // Act
        var expenditures = slice.GenerateExpenditures(referenceDate);

        // Assert
        user.Payments.Should().NotBeNull();
        user.Payments.Should().HaveCount(4);

        expenditures.Should().NotBeNull();
        expenditures.Should().NotBeEmpty();
        expenditures.Should().HaveCount(4);

        expenditures!.First().Name.Should().Be(pmtDescription);
        expenditures!.First().DueDate.Day.Should().Be(dueDay);
        expenditures!.First().DueDate.Month.Should().Be(referenceDate.Month);
        expenditures!.First().DueDate.Year.Should().Be(referenceDate.Year);
    }
}
