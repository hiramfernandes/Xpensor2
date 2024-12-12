namespace Xpensor2.Domain.Models;

public class ExecutedPayment
{
    public ExecutedPayment(string paymentMethod,
                           decimal paidValue,
                           DateTime paidDate,
                           DateTime paymentDueDate,
                           User paidBy)
    {
        PaymentMethod = paymentMethod;
        PaidValue = paidValue;
        PaidDate = paidDate;
        PaymentDueDate = paymentDueDate;
        PaidBy = paidBy;
    }

    public string PaymentMethod { get; }
    public decimal PaidValue { get; }
    public DateTime PaidDate { get; }
    public DateTime PaymentDueDate { get; }
    public User PaidBy { get; }
}
