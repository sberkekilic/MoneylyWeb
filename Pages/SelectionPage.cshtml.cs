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
        public async Task<IActionResult> OnGetAsync()
        {
            LoggedInEmail = HttpContext.Session.GetString("LoggedInEmail");
            IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";

            if (!IsLoggedIn || string.IsNullOrEmpty(LoggedInEmail))
            {
                return RedirectToPage("/Index");
            }

            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");
            var userFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(userFilePath) && System.IO.File.Exists(combinedDataFilePath))
            {
                try
                {
                    var userJsonData = await System.IO.File.ReadAllTextAsync(userFilePath);
                    var combinedDataJson = await System.IO.File.ReadAllTextAsync(combinedDataFilePath);

                    var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(userJsonData);
                    var combinedDataList = System.Text.Json.JsonSerializer.Deserialize<List<CombinedData>>(combinedDataJson) ?? new List<CombinedData>();

                    bool userExists = users?.Any(u => u.Email == LoggedInEmail) == true;
                    bool existingCombinedData = combinedDataList.Any(cd => cd.Email == LoggedInEmail);

                    if (userExists && existingCombinedData)
                    {
                        return RedirectToPage("HomePage");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error reading files: {ex.Message}");
                    return RedirectToPage("ErrorPage1");
                }
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
