using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Retrieve session values for login status and email
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
            var loggedInEmail = HttpContext.Session.GetString("LoggedInEmail");

            // Step 1: Check if the session is set to a logged-in state and email exists
            if (isLoggedIn && !string.IsNullOrEmpty(loggedInEmail))
            {
                // Step 2: Check if this email exists in combined_data.json
                var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

                if (System.IO.File.Exists(combinedDataFilePath))
                {
                    var jsonData = System.IO.File.ReadAllText(combinedDataFilePath);
                    var combinedDataList = JsonConvert.DeserializeObject<List<CombinedData>>(jsonData) ?? new List<CombinedData>();

                    // Step 3: Verify the user's email in the combined data
                    bool userExists = combinedDataList.Any(data => data.Email == loggedInEmail);

                    if (userExists)
                    {
                        // Redirect to HomePage if email is found
                        return RedirectToPage("/HomePage");
                    }
                }
            }

            // If session data is absent or user is not found in combined_data.json, stay on Index page
            return Page();
        }
    }
}
