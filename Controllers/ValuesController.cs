using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.json");

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var json = await System.IO.File.ReadAllTextAsync(_filePath);
            var userData = JsonConvert.DeserializeObject<User>(json);

            if (userData != null)
            {
                userData.isLoggedIn = false;

                var updatedJson = JsonConvert.SerializeObject(userData, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJson);

                return Ok("User logged out successfully");
            }

            return BadRequest("Failed to logout");
        }
    }
}
