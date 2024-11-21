namespace Xpensor2.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal? Value { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string? Owner { get; set; }
    }
}
