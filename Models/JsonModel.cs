namespace WebApplication1.Models
{
    public class JsonModel
    {
        public string Email { get; set; }
        public string SelectedOption { get; set; }
        public double IncomeAmount { get; set; }
        public Subscription Subscription { get; set; }
        public Bills Bills { get; set; }
        public Others Others { get; set; }
    }
}
