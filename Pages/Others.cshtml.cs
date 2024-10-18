using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        public string Email { get; set; }
        [BindProperty]
        public string SelectedOption { get; set; }

        [BindProperty]
        public double IncomeAmount { get; set; }
        public string LoggedInEmail { get; set; }

        private string GetEmailFromJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");
            var jsonData = System.IO.File.ReadAllText(filePath);
            var jsonObject = JsonSerializer.Deserialize<JsonModel>(jsonData);
            return jsonObject.Email; // Assuming your JSON structure has Email property
        }

        public IActionResult OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                var storedUserData = JsonSerializer.Deserialize<User>(jsonData);
                bool isLoggedIn = storedUserData.isLoggedIn;

                if (!isLoggedIn)
                {
                    Response.Redirect("/Index");
                }
            }
            LoggedInEmail = HttpContext.Session.GetString("LoggedInEmail");

            // Load the email from JSON
            string emailFromJson = GetEmailFromJson();
            if (LoggedInEmail == emailFromJson)
            {
                // Redirect to HomePage if the logged-in email matches the email from JSON
                return RedirectToPage("/HomePage");
            }

            return Page();
        }
        public IActionResult OnPost(string selectedOption, double incomeAmount)
        {
            LoggedInEmail = HttpContext.Session.GetString("LoggedInEmail");

            _logger.LogInformation("Others form submitted.");
            _logger.LogInformation("IncomeAmount in Others: {IncomeAmount}", incomeAmount);
            _logger.LogInformation("Email in Others: {Email}", LoggedInEmail);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                return Page(); // Re-render the page with errors
            }

            if (TempData.TryGetValue("SubsData", out var subsDataJson) && TempData.TryGetValue("BillsData", out var billsDataJson))
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
                    Email = LoggedInEmail,
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
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                    return Page(); // Return the page with error message
                }

                TempData.Remove("SubsData");
                TempData.Remove("BillsData");

                return RedirectToPage("HomePage"); // Redirect after successful post
            }
            else
            {
                _logger.LogWarning("Subscription or Bills data missing.");
                ModelState.AddModelError(string.Empty, "Subscription or Bills data is missing.");
                return Page();
            }
        }

    }
}
