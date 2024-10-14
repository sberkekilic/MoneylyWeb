using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Others
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public DateTime? PeriodDate { get; set; }

        public DateTime? DueDate { get; set; } // Optional
    }
}
