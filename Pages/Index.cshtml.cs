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
            // First check login status using session
            bool isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";

            // If not logged in by session, check users.json as a fallback
            if (!isLoggedIn)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

                if (System.IO.File.Exists(filePath))
                {
                    var jsonData = System.IO.File.ReadAllText(filePath);
                    var storedUserData = JsonConvert.DeserializeObject<JArray>(jsonData);

                    foreach (var user in storedUserData)
                    {
                        if (user["isLoggedIn"] != null && user["isLoggedIn"].Value<bool>())
                        {
                            isLoggedIn = true;
                            HttpContext.Session.SetString("IsLoggedIn", "true");
                            break;
                        }
                    }
                }

              
            }
            
            else
            {
                Response.Redirect("/SelectionPage");
            }


            return Page();

        }
    }
}
