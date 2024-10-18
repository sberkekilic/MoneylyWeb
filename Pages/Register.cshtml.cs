using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace WebApplication1.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userData = new { Email, Password, isLoggedIn = true};
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            string jsonString = JsonSerializer.Serialize(userData, new JsonSerializerOptions { WriteIndented = true });

            await System.IO.File.WriteAllTextAsync(filePath, jsonString);

            return RedirectToPage("Login");
        }
    }
}
