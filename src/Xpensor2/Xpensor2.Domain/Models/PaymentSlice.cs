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
        return Owner.Payments?
                .Select(pmt => MapFrom(payment: pmt,
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
