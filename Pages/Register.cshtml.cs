using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Models;

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
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            // Read existing users
            List<User> users;
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
            }
            else
            {
                users = new List<User>();
            }

            // Check if the email already exists
            if (users.Any(u => u.Email == Email))
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }

            // Add new user
            var newUser = new User { Email = Email, Password = Password, isLoggedIn = true };
            users.Add(newUser);

            // Write updated users list back to JSON
            string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(filePath, jsonString);

            return RedirectToPage("Login2");
        }
    }
}
