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
        var pmtDescription = "Pmt1";
        var dueDay = 5;

        // Recurring payments (no end date - subscriptions, rent, ...)
        var pmt = user.CreatePayment(description: pmtDescription, nominalValue: 150, dueDay: dueDay);
        var pmt2 = user.CreatePayment(description: "Pmt2", nominalValue: 100, dueDay: 2);
        var pmt3 = user.CreatePayment(description: "Pmt3", nominalValue: 200, dueDay: 5);
        var pmt4 = user.CreatePayment(description: "Pmt4", nominalValue: 300, dueDay: 10);

        var installment = user.CreatePayment

        var slice = new PaymentSlice(user);
        var referenceDate = DateTime.Today;

        // Execute Payments and ensure their status change on the report
        // Pay the first one
        pmt.ExecutePayment(
            new ExecutedPayment() 
            { 
                PaidBy = user, 
                PaidDate = referenceDate.AddDays(1), 
                PaymentDueDate = referenceDate, 
                PaymentMethod = "Cash" 
            });

        // Pay the second one but from a previous month - should not gerenate a paidDate for the current month
        pmt2.ExecutePayment(
            new ExecutedPayment()
            {
                PaidBy = user,
                PaidDate = referenceDate.AddDays(1),
                PaymentDueDate = referenceDate.AddMonths(-2),
                PaymentMethod = "Card"
            });

        // Act
        var expenditures = slice.MonthlyReport(referenceDate);

        // Assert
        user.Payments.Should().NotBeNull();
        user.Payments.Should().HaveCount(4);

        expenditures.Should().NotBeNull();
        expenditures.Should().NotBeEmpty();
        expenditures.Should().HaveCount(4);

        expenditures!.ElementAt(0).Name.Should().Be(pmtDescription);
        expenditures!.ElementAt(0).DueDate.Day.Should().Be(dueDay);
        expenditures!.ElementAt(0).DueDate.Month.Should().Be(referenceDate.Month);
        expenditures!.ElementAt(0).DueDate.Year.Should().Be(referenceDate.Year);
        expenditures!.ElementAt(0).PaymentDate.Should().NotBeNull();
        expenditures!.ElementAt(0).PaymentDate!.Value.Month.Should().Be(referenceDate.Month);
        expenditures!.ElementAt(0).PaymentDate!.Value.Year.Should().Be(referenceDate.Year);

        expenditures!.ElementAt(1).Name.Should().Be("Pmt2");
        expenditures!.ElementAt(1).DueDate.Day.Should().Be(2);
        expenditures!.ElementAt(1).DueDate.Month.Should().Be(referenceDate.Month);
        expenditures!.ElementAt(1).DueDate.Year.Should().Be(referenceDate.Year);
        expenditures!.ElementAt(1).PaymentDate.Should().BeNull();
    }
}
