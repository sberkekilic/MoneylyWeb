using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var jsonObject = System.Text.Json.JsonSerializer.Deserialize<JsonModel>(jsonData);
            return jsonObject.Email; // Assuming your JSON structure has Email property
        }

        public IActionResult OnGet()
        {
            var userFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            // Check if users.json exists
            if (System.IO.File.Exists(userFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(userFilePath);
                var storedUserData = JsonConvert.DeserializeObject<JArray>(jsonData);
                bool isLoggedIn = false;

                foreach (var user in storedUserData)
                {
                    if (user["isLoggedIn"] != null && user["isLoggedIn"].Value<bool>())
                    {
                        isLoggedIn = true;
                        break;
                    }
                }

                if (!isLoggedIn)
                {
                    Response.Redirect("/Index");
                }
            }
            LoggedInEmail = HttpContext.Session.GetString("LoggedInEmail");

            // Check if combined_data.json exists before calling GetEmailFromJson
            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");
            if (System.IO.File.Exists(combinedDataFilePath))
            {
                string emailFromJson = GetEmailFromJson();
                if (LoggedInEmail == emailFromJson)
                {
                    // Redirect to HomePage if the logged-in email matches the email from JSON
                    return RedirectToPage("/HomePage1");
                }
            }
            else
            {
                _logger.LogWarning("combined_data.json file does not exist.");
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
                var subsData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(subsDataJson.ToString());
                var billsData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(billsDataJson.ToString());

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
                    System.IO.File.WriteAllText(filePath, System.Text.Json.JsonSerializer.Serialize(combinedData, new JsonSerializerOptions { WriteIndented = true }));
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

                return RedirectToPage("HomePage2"); // Redirect after successful post
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
