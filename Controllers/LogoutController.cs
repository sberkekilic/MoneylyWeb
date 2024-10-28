using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LogoutRequest request)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("User data not found.");
            }

            var userJsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<User>>(userJsonData);

            var user = users.FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                user.isLoggedIn = false; // Set isLoggedIn to false
                                         // Optionally, you can also set other fields to null or empty
            }

            // Save the updated user data back to the file
            var updatedUserJsonData = JsonSerializer.Serialize(users);
            await System.IO.File.WriteAllTextAsync(filePath, updatedUserJsonData);

            HttpContext.Session.Remove("IsLoggedIn");
            
            return Ok(new {message = "Logout Successful"});
        }
    }
}