using Xpensor2.Domain.Models.Enums;

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
        return Owner.Expenditures?
                //.Where(x => x.ExecutedPayment == null)
                .Where(x => x.DueDate.Month == referenceDate.Month && x.DueDate.Year == referenceDate.Year)
                .Where(x => x.ExecutedPayment == null);

        // Need to check:
        // 3) If there's some payment left behind

        // Recurring
        return Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Recurring)
            .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year));

        // Single
        return Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Single)
            .Where(x => x.DueDate.Month == referenceDate.Month && x.DueDate.Year == referenceDate.Year)
            .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year));

        // Installments
        return Owner.Payments
            .Where(x => x.PaymentType == PaymentType.Installment)
            .Where(x => x.StartDate.HasValue)
            .Where(x => x.StartDate!.Value.Month >= referenceDate.Month) // Improve filter to return the ones that matches the criteria - pipe key not found on my keyboard
            .Select(x => MapFrom(x, referenceDate.Month, referenceDate.Year));
    }

    //public static DateTime? GetExecutedPaymentFor(Payment payment, int month, int year)
    //{
    //    //return payment.ExecutedPayment?.PaidDate;
    //}

    private static Expenditure MapFrom(Payment payment, int month, int year)
    {
        //var paidDate = GetExecutedPaymentFor(payment, month, year);
        return new Expenditure()
        {
            DueDate = new DateTime(year, month, payment.DueDay),
            Name = payment.Description,
            GeneralInfo = string.Empty,
            //PaymentDate = paidDate
        };
    }
}
