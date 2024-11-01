using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApplication1.Pages
{
    public class HomePageModel : PageModel
    {
        public List<CombinedData> JsonData { get; set; } = new List<CombinedData>();
        public async Task<IActionResult> OnGetAsync()
        {
            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

            if (System.IO.File.Exists(combinedDataFilePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(combinedDataFilePath);
                var allData = JsonConvert.DeserializeObject<List<CombinedData>>(jsonData) ?? new List<CombinedData>();
                var userEmail = HttpContext.Session.GetString("LoggedInEmail");

                // Check if userEmail is not null
                if (!string.IsNullOrEmpty(userEmail))
                {
                    // Filter data for the logged-in user
                    JsonData = allData.Where(d => d.Email == userEmail).ToList();
                }


            }

            return Page();
        }
    }
}
