using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class OthersModel : PageModel
    {
        private readonly ILogger<OthersModel> _logger;

        public OthersModel(ILogger<OthersModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public Others Other { get; set; }

        [BindProperty]
        public string SelectedOption { get; set; }

        [BindProperty]
        public double IncomeAmount { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost(string selectedOption, double incomeAmount)
        {

            _logger.LogInformation("Others form submitted.");
            _logger.LogInformation("IncomeAmount in Others: {IncomeAmount}", incomeAmount);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model is valid, starting file write process.");

                if (
                    TempData.TryGetValue("SubsData", out var subsDataJson) 
                    && 
                    TempData.TryGetValue("BillsData", out var billsDataJson) 
                    && 
                    subsDataJson != null 
                    && 
                    billsDataJson != null
                    )
                {
                    var subsData = JsonSerializer.Deserialize<dynamic>(subsDataJson.ToString());
                    var billsData = JsonSerializer.Deserialize<dynamic>(billsDataJson.ToString());

                    var othersData = new
                    {
                        Other.Name,
                        Other.Amount,
                        Other.PeriodDate,
                        Other.DueDate
                    };

                    var combinedData = new
                    {
                        SelectedOption = selectedOption,
                        IncomeAmount = incomeAmount,
                        Subscription = subsData,
                        Bills = billsData,
                        Others = othersData
                    };

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

                    try
                    {
                        System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(combinedData, new JsonSerializerOptions { WriteIndented = true }));

                        _logger.LogInformation("Data successfully written to combined_data.json");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while writing to combined_data.json");
                        Console.WriteLine($"Error writing to file: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                        _logger.LogError($"File path attempted: {filePath}");
                        return Page();
                    }

                    TempData.Remove("SubsData");
                    TempData.Remove("BillsData");

                    return RedirectToPage("HomePage");
                }
                else
                {
                    _logger.LogWarning("Subscription data is missing. Please complete the subscription form first.");
                    ModelState.AddModelError(string.Empty, "Subscription data is missing. Please complete the subscription form first.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
