using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class SubsModel : PageModel
    {
        private readonly ILogger<SubsModel> _logger;
        public SubsModel(ILogger<SubsModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public Subscription Subscription { get; set; }

        [BindProperty]
        public string SelectedOption { get; set; }

        [BindProperty]
        public double IncomeAmount { get; set; }
        public IActionResult OnGet(string selectedOption, double incomeAmount)
        {
            SelectedOption = selectedOption;
            IncomeAmount = incomeAmount;

            _logger.LogInformation("SelectedOption: {SelectedOption}", SelectedOption);
            _logger.LogInformation("IncomeAmount: {IncomeAmount}", IncomeAmount);

            return Page();
        }

        public IActionResult OnPost()
        {
            _logger.LogInformation("Subscription form submitted.");
            _logger.LogInformation("IncomeAmount in Subs: {IncomeAmount}", IncomeAmount);

            if (ModelState.IsValid)
            {
                // Use parsedIncomeAmount instead of incomeAmount here
                var subsData = new
                {
                    Subscription.Name,
                    Subscription.Amount,
                    Subscription.PeriodDate,
                    Subscription.DueDate,
                };

                TempData["SubsData"] = JsonSerializer.Serialize(subsData);

                return RedirectToPage("Bills");
            }

            ModelState.AddModelError("incomeAmount", "Invalid income amount.");
            _logger.LogWarning("Invalid income amount.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Model error: {ErrorMessage}", error.ErrorMessage);
            }

            return Page();
        }

    }
}
