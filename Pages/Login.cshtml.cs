using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
       
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");
            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

            if (System.IO.File.Exists(userFilePath))
            {
                // Read and deserialize the list of users
                var userJsonData = await System.IO.File.ReadAllTextAsync(userFilePath);
                var users = JsonSerializer.Deserialize<List<User>>(userJsonData);

                // Find the user with the matching email and password
                var storedUser = users?.FirstOrDefault(u => u.Email == Email && u.Password == Password);

                if (storedUser != null)
                {
                    // Update the user's login state
                    storedUser.isLoggedIn = true;

                    // Serialize the updated user list back to JSON
                    string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                    await System.IO.File.WriteAllTextAsync(userFilePath, updatedJson);

                    // Store email in session after successful login
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("LoggedInEmail", Email);
                    TempData["Email"] = Email;
                    TempData["IsLoggedIn"] = "true";

                    // Return a JSON response indicating success
                    return new JsonResult(new { success = true, email = Email });
                }
                else
                {
                    // User not found or incorrect password
                    return new JsonResult(new { success = false, errorMessage = "Incorrect email or password." });
                }
            }
            else
            {
                // Handle case where users.json does not exist
                return new JsonResult(new { success = false, errorMessage = "No registered users found." });
            }
        }
    }

    public class CombinedData
    {
        public string Email { get; set; }
        public string SelectedOption { get; set; }
        public decimal IncomeAmount { get; set; }
        public Subscription Subscription { get; set; }
        public Bills Bills { get; set; }
        public Others Others { get; set; }
    }
}
