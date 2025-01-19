using FluentAssertions;
using Xpensor2.Domain.Models;

namespace Xpensor2.Domain.Test;

public class PaymentTest
{
    [Fact]
    public void GenerateExpenses_ShouldCreateExpenses_BasedOnExistingPayments()
    {
        // Arrange
        var user = new User("name");
        var pmtDescription = "Pmt1";
        var dueDay = 5;

        // Recurring payments (no end date - subscriptions, rent, ...)
        var pmt = user.CreateRecurringPayment(description: pmtDescription, nominalValue: 150, dueDay: dueDay);
        var pmt2 = user.CreateRecurringPayment(description: "Pmt2", nominalValue: 100, dueDay: 2);
        var pmt3 = user.CreateRecurringPayment(description: "Pmt3", nominalValue: 200, dueDay: 5);
        var pmt4 = user.CreateRecurringPayment(description: "Pmt4", nominalValue: 300, dueDay: 10);

        var installmentDescription = "Installment1";
        var installmentValue = 123;
        var installment = user.CreateInstallment(installmentDescription, installmentValue, 4, 5, DateTime.Today.AddMonths(-3));

        var slice = new PaymentSlice(user, new InMemoryExpenditureRepository(user));
        var referenceDate = DateTime.Today;

        // Execute Payments and ensure their status change on the report
        // Pay the first one
        //pmt.ExecutePayment(new ExecutedPayment("Cash", referenceDate.AddDays(1), referenceDate, user));

        // Pay the second one but from a previous month - should not gerenate a paidDate for the current month
        //pmt2.ExecutePayment(new ExecutedPayment("Card", referenceDate.AddDays(1), referenceDate.AddMonths(-2), user));

        // Act
        var expenditures = slice.MonthlyReport(referenceDate);

        // Assert
        user.Payments.Should().NotBeNull();
        user.Payments.Should().HaveCount(5);

        expenditures.Should().NotBeNull();
        expenditures.Should().NotBeEmpty();
        expenditures.Should().HaveCount(5);

        expenditures!.ElementAt(0).Name.Should().Be(pmtDescription);
        expenditures!.ElementAt(0).DueDate.Day.Should().Be(dueDay);
        expenditures!.ElementAt(0).DueDate.Month.Should().Be(referenceDate.Month);
        expenditures!.ElementAt(0).DueDate.Year.Should().Be(referenceDate.Year);

        expenditures!.ElementAt(1).Name.Should().Be("Pmt2");
        expenditures!.ElementAt(1).DueDate.Day.Should().Be(2);
        expenditures!.ElementAt(1).DueDate.Month.Should().Be(referenceDate.Month);
        expenditures!.ElementAt(1).DueDate.Year.Should().Be(referenceDate.Year);
    }

    [Fact]
    public void Installments_ShouldOnlyGenerateExpenditures_WhenDuringValidPeriod()
    {
        // Arrange
        var user = new User("Installment User");
        var installmentStartingMonth = 12;
        var installmentStartingYear = 2024;
        var installmentStartDate = new DateTime(installmentStartingYear, installmentStartingMonth, 5);

        var fiveMonthInstallment = user.CreateInstallment("Some purchase split in five pmts", 100, 5, 5, installmentStartDate);

        // Act
        var slice = new PaymentSlice(user, new InMemoryExpenditureRepository(user));

        // Previous month to the beginning of the installment should generate no expenses
        var expenditures = slice.MonthlyReport(new DateTime(installmentStartingYear, installmentStartingMonth - 1, 1));
        expenditures.Should().NotBeNull();
        expenditures!.Should().BeEmpty();

        // Current month when the installment starts should generate a single expense
        expenditures = slice.MonthlyReport(new DateTime(installmentStartingYear, installmentStartingMonth, 1));
        expenditures.Should().NotBeNull();
        expenditures.Should().HaveCount(1);

        // 3 months after the installment has started should also generate a single expense
        expenditures = slice.MonthlyReport(new DateTime(installmentStartingYear + 1, installmentStartingMonth - 12 + 3, 1));
        expenditures.Should().NotBeNull();
        expenditures.Should().HaveCount(1);

        // Fifth and last report that should contain the expenditure
        expenditures = slice.MonthlyReport(new DateTime(installmentStartingYear + 1, installmentStartingMonth - 12 + 4, 1));
        expenditures.Should().NotBeNull();
        expenditures.Should().HaveCount(1);

        // 6 months after the installment has started should also generate no expenses
        expenditures = slice.MonthlyReport(new DateTime(installmentStartingYear + 1, installmentStartingMonth - 12 + 5, 1));
        expenditures.Should().NotBeNull();
        expenditures.Should().BeEmpty();
    }

    [Fact]
    public void Installments_ShouldNotAppear_WhenAllPaymentsExecuted()
    {
        // Arrange
        var user = new User("user");
        var slice = new PaymentSlice(user, new InMemoryExpenditureRepository(user));
        var installmentStartDate = new DateTime(2024, 12, 8);

        // Act
        var generatedInstallment = 
            user.CreateInstallment(
                description: "installment1",
                installmentValue: 100,
                numberOfInstallments: 5,
                dueDay: 5,
                startDate: installmentStartDate);

        var expenditures = slice.MonthlyReport(installmentStartDate);

        // Before payments there should be 1 expenditure (one for each month starting in December - which is the selected one by date)
        expenditures.Should().NotBeNull();
        expenditures.Should().NotBeEmpty();
        expenditures.Should().HaveCount(1);

        // Adding a payment for December and re-calculating expenditures
        var expenditure = expenditures?.FirstOrDefault();
        expenditure?.Should().NotBeNull();
        user.RegisterExecutedPayment(expenditure!, new ExecutedPayment("Cash", 123, expenditure!.DueDate, user));
        expenditures = slice.MonthlyReport(installmentStartDate);
        expenditures.Should().HaveCount(0);
    }
}
