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
        // Payments that haven't been paid and are due during the reference period
        var payments = Owner.Payments?
                .Where(x => x.ExecutedPayment == null)
                .Where(x => x.DueDate.Month == referenceDate.Month && x.DueDate.Year == referenceDate.Year);

        // Need to check:
        // 3) If there's some payment left behind

        return payments?.Select(instllm => MapFrom(payment: instllm,
                                            month: referenceDate.Month,
                                            year: referenceDate.Year));
    }

    public static DateTime? GetExecutedPaymentFor(Payment payment, int month, int year)
    {
        return payment.ExecutedPayment?.PaidDate;
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
