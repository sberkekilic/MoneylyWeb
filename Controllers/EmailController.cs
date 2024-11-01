using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Pages;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("validate-email")]
        public IActionResult ValidateEmail([FromBody] EmailRequest request)
        {
            var combinedDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");

            if (System.IO.File.Exists(combinedDataFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(combinedDataFilePath);
                var allData = JsonConvert.DeserializeObject<List<CombinedData>>(jsonData) ?? new List<CombinedData>();

                // Check if the email exists in the data
                if (allData.Any(d => d.Email == request.Email))
                {
                    // Optionally, set the email in session if needed
                    HttpContext.Session.SetString("LoggedInEmail", request.Email);
                    return Ok();
                }
            }
            return NotFound("Email not found.");
        }
        }

    public class EmailRequest
    {
        public string Email { get; set; }
    }
}
