using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {

        public void OnGet()
        {
            // Check login status
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                var storedUserData = JsonConvert.DeserializeObject<dynamic>(jsonData);
                bool isLoggedIn = storedUserData.isLoggedIn;

                if (isLoggedIn)
                {
                    // Redirect to HomePage
                    Response.Redirect("/HomePage");
                }
            }
        }
    }
}
