namespace Xpensor2.Domain.Models
{
    public class Payment
    {
        public Payment(string description, User owner, decimal nominalValue, int dueDay)
        {
            Id = Guid.NewGuid();
            Description = description;
            Owner = owner;
            NominalValue = nominalValue;
            DueDay = dueDay;
            Recurrence = PaymentRecurrence.Monthly;
        }

        public Guid Id { get; set; }
        public string Description { get; init; }
        public decimal? NominalValue { get; set; }
        public DateTime DueDate { get; set; }
        public int DueDay { get; set; }
        public PaymentRecurrence Recurrence { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public User? Owner { get; set; }
    }

    public enum PaymentRecurrence
    {
        Monthly
    }

    public class PaymentSlice
    {
        public PaymentSlice() => Payments = [];

        public List<Payment>? Payments { get; private set; }

        public void AddPayment(Payment payment)
        {
            Payments!.Add(payment);
        }

        public IEnumerable<Expenditure>? GenerateExpenditures(DateTime referenceDate)
        {
            // Regular Payments (no end date)
            return Payments!
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

    public class Expenditure
    {
        public string? Name { get; set; }
        public DateTime DueDate { get; set; }
        public string? GeneralInfo { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    public class User
    {
        public Guid Id { get; init; }
        public string? Name { get; set; }

        public Payment CreatePayment(string description, decimal nominalValue, int dueDay)
        {
            return new Payment(description, this, nominalValue, dueDay);
        }
    }
}
