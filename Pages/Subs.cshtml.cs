using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class SubsModel : PageModel
    {
        [BindProperty]
        public Subscription Subscription { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string selectedOption)
        {
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

            return Page();
        }
    }
}
