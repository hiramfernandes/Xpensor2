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
        // Regular Payments (no end date)
        var payments = Owner.Payments?
                .Where(x => x.NumberOfInstallments == null).ToList();

        // Installments (finite number of payments)
        // Need to check:
        // 1) If the installments haven't already been paid
        // 2) If there's something pending for the month (Start Date + NumINstallments = this month)
        // 3) If there's some payment left behind
        var installments = Owner.Payments?
                .Where(x => x.NumberOfInstallments != null)
                //.Where(x => x.StartDate >= referenceDate.Date)
                .ToList();

        payments?.AddRange(installments ?? []);

        return payments?.Select(instllm => MapFrom(payment: instllm,
                                            month: referenceDate.Month,
                                            year: referenceDate.Year));
    }

    public static DateTime? GetExecutedPaymentFor(Payment payment, int month, int year)
    {
        var executedPaymentForReferredDate = payment.ExecutedPayments
            .FirstOrDefault(x => x.PaymentDueDate.Month == month &&
                                 x.PaymentDueDate.Year == year);

        return executedPaymentForReferredDate?.PaidDate;
    }

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        var paidDate = GetExecutedPaymentFor(payment, month, year);
        return new Expenditure()
        {
            DueDate = new DateTime(year, month, payment.DueDay),
            Name = payment.Description,
            GeneralInfo = string.Empty,
            PaymentDate = paidDate
        };
    }
}
