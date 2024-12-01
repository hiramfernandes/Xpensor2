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

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        return new Expenditure()
        {
            DueDate = new DateTime(year, month, payment.DueDay),
            Name = payment.Description,
            GeneralInfo = string.Empty
        };
    }
}
