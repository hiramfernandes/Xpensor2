namespace Xpensor2.Domain.Models
{
    public class Payment
    {
        public Payment(string description, User owner)
        {
            Id = Guid.NewGuid();
            Description = description;
            Owner = owner;
        }

        public Guid Id { get; set; }
        public string Description { get; init; }
        public decimal? Value { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public User? Owner { get; set; }
    }

    public class PaymentSlice
    {
        private List<Expenditure> _expenditures;

        public PaymentSlice()
        {
            _expenditures = new List<Expenditure>();
        }

        public DateTime SliceStart { get; set; }
        public DateTime SliceEnd { get; set; }
        public IEnumerable<Expenditure>? Expenditures { get; set; }

        public IEnumerable<Expenditure>? GenerateSlice(DateTime from, DateTime to)
        {
            return _expenditures.Where(x => x.DueDate >= from && x.DueDate <= to);
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

        public void CreatePayment(string description)
        {
            var payment = new Payment(description, this);
        }
    }
}
