namespace Xpensor2.Domain.Models
{
    public class Payment
    {
        public Payment(string description, User owner, decimal value)
        {
            Id = Guid.NewGuid();
            Description = description;
            Owner = owner;
            NominalValue = value;
        }

        public Guid Id { get; set; }
        public string Description { get; init; }
        public decimal? NominalValue { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public User? Owner { get; set; }
    }

    public class PaymentSlice
    {
        public PaymentSlice() => Payments = [];

        public DateTime SliceStart { get; set; }
        public DateTime SliceEnd { get; set; }
        public List<Payment>? Payments { get; private set; }

        public void AddPayment(Payment payment)
        {
            Payments!.Add(payment);
        }

        public IEnumerable<Expenditure>? GenerateExpenditures(DateTime from, DateTime to)
        {
            return Payments!
                .Where(x => x.DueDate >= from && x.DueDate <= to)
                .Select(x => new Expenditure() { DueDate = x.DueDate } );
        }
    }

    public class Expenditure
    {
        public string? Name { get; set; }
        public DateTime DueDate { get; set;}
    }

    public class User
    {
        public Guid Id { get; init; }
        public string? Name { get; set; }

        public Payment CreatePayment(string description, decimal value)
        {
            return new Payment(description, this, value);
        }
    }
}
