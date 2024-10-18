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
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                var storedUserData = JsonSerializer.Deserialize<User>(jsonData);

                if (storedUserData.Email == Email && storedUserData.Password == Password)
                {
                    storedUserData.isLoggedIn = true;

                    string updatedJson = JsonSerializer.Serialize(storedUserData, new JsonSerializerOptions { WriteIndented = true });
                    await System.IO.File.WriteAllTextAsync(filePath, updatedJson);
                    // Store email in session after successful login
                    HttpContext.Session.SetString("LoggedInEmail", Email);
                    return RedirectToPage("SelectionPage");
                }
                else
                {
                    ErrorMessage = "Incorrect email or password.";
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "No registered users found.";
                return Page();
            }
        }
    }
}
