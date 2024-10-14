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
        public void OnGet()
        {
        }

        public IActionResult OnPost(string selectedOption)
        {
            _logger.LogInformation("Subscription form submitted.");

            if (ModelState.IsValid)
            {
                var subsData = new
                {
                    Subscription.Name,
                    Subscription.Amount,
                    Subscription.PeriodDate,
                    Subscription.DueDate,
                    SelectedOption = selectedOption
                };

                TempData["SubsData"] = JsonSerializer.Serialize(subsData);

                return RedirectToPage("Bills");
            }

            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return Page();
        }
    }
}
