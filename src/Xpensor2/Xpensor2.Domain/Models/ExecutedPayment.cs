namespace Xpensor2.Domain.Models;

public class ExecutedPayment
{
    public ExecutedPayment(string paymentMethod, DateTime paidDate, DateTime paymentDueDate, User paidBy)
    {
        PaymentMethod = paymentMethod;
        PaidDate = paidDate;
        PaymentDueDate = paymentDueDate;
        PaidBy = paidBy;
    }

    public string PaymentMethod { get; }
    public DateTime PaidDate { get; }
    public DateTime PaymentDueDate { get; }
    public User PaidBy { get; }
}
