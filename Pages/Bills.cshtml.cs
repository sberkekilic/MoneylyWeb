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
                        SelectedOption = selectedOption
                    };

                    var combinedData = new
                    {
                        Subscription = subsData,
                        Bills = billsData
                    };

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

                    try
                    {
                        System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(combinedData, new JsonSerializerOptions { WriteIndented = true }));
                        _logger.LogInformation("Data successfully written to combined_data.json");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception for debugging
                        _logger.LogError(ex, "An error occurred while writing to combined_data.json");
                        Console.WriteLine($"Error writing to file: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                        return Page();
                    }

                    // Optionally clear TempData after use
                    TempData.Remove("SubsData");

                    // Redirect to a success page or the Bills page
                    return RedirectToPage("Success");
                }
                else
                {
                    // Handle the case where TempData is not available
                    _logger.LogWarning("Subscription data is missing. Please complete the subscription form first.");
                    ModelState.AddModelError(string.Empty, "Subscription data is missing. Please complete the subscription form first.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
