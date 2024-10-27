using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Check login status using session first
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                Response.Redirect("/SelectionPage");
                return;
            }

            // If session is not set, check the users.json file as fallback
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
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

                if (isLoggedIn)
                {
                    Response.Redirect("/SelectionPage");
                }
                else
                {
                    Response.Redirect("/Login");
                }
            }
            else
            {
                // If no users.json file exists, redirect to Login page
                Response.Redirect("/Login");
            }
        }
    }
}
