using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class BillsModel : PageModel
    {
        private readonly ILogger<BillsModel> _logger;
        public BillsModel(ILogger<BillsModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public Bills Bill { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string selectedOption)
        {
            _logger.LogInformation("Bills form submitted.");

            if (ModelState.IsValid)
            {
                // Check if TempData contains the subscription data
                if (TempData.TryGetValue("SubsData", out var subsDataJson) && subsDataJson != null)
                {
                    var subsData = JsonSerializer.Deserialize<dynamic>(subsDataJson.ToString());

                    var billsData = new
                    {
                        Bill.Name,
                        Bill.Amount,
                        Bill.PeriodDate,
                        Bill.DueDate,
                    };

                    TempData["BillsData"] = JsonSerializer.Serialize(billsData);

                    return RedirectToPage("Others");
                }
                else
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning("Model error: {ErrorMessage}", error.ErrorMessage);
                    }
                }
            }

            return Page();
        }
    }
}
