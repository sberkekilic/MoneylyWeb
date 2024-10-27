using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class SelectionPageModel : PageModel
    {
        public string LoggedInEmail { get; private set; }
        public bool IsLoggedIn { get; private set; }
        public async Task<IActionResult> OnGetAsync(string isLoggedIn)
        {
            LoggedInEmail = HttpContext.Session.GetString("LoggedInEmail");
            IsLoggedIn = !string.IsNullOrEmpty(isLoggedIn) && isLoggedIn.Equals("true", StringComparison.OrdinalIgnoreCase);

            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");
            var userFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(userFilePath) && System.IO.File.Exists(combinedDataFilePath))
            {
                var userJsonData = await System.IO.File.ReadAllTextAsync(userFilePath);
                var combinedDataJson = await System.IO.File.ReadAllTextAsync(combinedDataFilePath);

                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(userJsonData);
                var combinedData = System.Text.Json.JsonSerializer.Deserialize<CombinedData>(combinedDataJson);

                var userExists = users?.Any(u => u.Email == LoggedInEmail) == true;

                if (IsLoggedIn && userExists && combinedData != null && combinedData.Email == LoggedInEmail)
                {
                    // Redirect to HomePage if email matches
                    return RedirectToPage("HomePage"); // Replace with your actual HomePage
                }
            }
            else
            {
                return RedirectToPage("ErrorPage");
            }

            return Page();

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("Subs"); 
        }
    }
}
