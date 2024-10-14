namespace WebApplication1.Models
{
    public class Subscription
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime PeriodDate { get; set; }
        public DateTime? DueDate { get; set; } // Nullable for optional
    }
}
