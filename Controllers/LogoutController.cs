using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                var userData = JsonSerializer.Deserialize<User>(jsonData);

                // Set isLoggedIn to false
                userData.isLoggedIn = false;

                string updatedJson = JsonSerializer.Serialize(userData, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(filePath, updatedJson);

                return Ok();
            }
            return NotFound("User data not found.");
        }
    }
}
