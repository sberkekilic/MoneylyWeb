namespace WebApplication1.Models
{
    public class Bills
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime PeriodDate { get; set; }
        public DateTime? DueDate { get; set; } // Optional
    }
}
