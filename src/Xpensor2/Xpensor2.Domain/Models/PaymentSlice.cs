﻿using Xpensor2.Domain.Models.Enums;

namespace Xpensor2.Domain.Models;

public class PaymentSlice
{
    public PaymentSlice(User owner)
    {
        Owner = owner;
    }

    public User Owner { get; set; }

    public IEnumerable<Expenditure>? MonthlyReport(DateTime referenceDate)
    {
        // From Payments you can get to Expenditures in three steps:
        /// 1) Recurring payments - every iteration generates them as long as they are active/enabled
        /// 2) Installments - a bit more tricky, but based on the first installment count to the current month to see if there are pending ones
        /// 3) Single - just check if the due date belongs to the current exercise

        // Payments that haven't been paid and are due during the reference period
        // Need to check:
        // 3) If there's some payment left behind

        // Recurring
        var recurring = Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Recurring);

        // Single
        var single = Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Single)
            .Where(x => x.DueDate.Month == referenceDate.Month && x.DueDate.Year == referenceDate.Year);

        // Installments
        var installments = Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Installment)
            .Where(x => x.StartDate.HasValue && x.NumberOfInstallments.HasValue)
            .Where(x => BelongsToTheInstallmentRange(referenceDate, x.StartDate!.Value, x.NumberOfInstallments!.Value))
            .Where(x => !Owner.Expenditures.Any(y => y.Payment.Id == x.Id && y.ExecutedPayment != null));

        var monthlyExpenses = recurring.Concat(single).Concat(installments).Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year)).ToList();
        Owner.Expenditures.AddRange(monthlyExpenses);

        return monthlyExpenses;
    }

    private bool BelongsToTheInstallmentRange(DateTime referenceDate, DateTime installmentStartDate, int numberOfInstallments)
    {
        // Get first day of month for both dates
        var firstDayOfTheMonthReferenceDate = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var firstDayOfTheMonthInstallmentStartDate = new DateTime(installmentStartDate.Year, installmentStartDate.Month, 1);

        // Before the beginning of the installment period
        if (firstDayOfTheMonthReferenceDate >= firstDayOfTheMonthInstallmentStartDate &&
            firstDayOfTheMonthReferenceDate <= firstDayOfTheMonthInstallmentStartDate.AddMonths(numberOfInstallments - 1))
            return true;

        return false;
    }

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        var dueDate = new DateTime(year, month, payment.DueDay);
        return new Expenditure(payment, dueDate, payment.Description, string.Empty);
    }
}
