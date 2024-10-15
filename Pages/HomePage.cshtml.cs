using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApplication1.Pages
{
    public class HomePageModel : PageModel
    {

        public dynamic JsonData { get; set; }
        public void OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "combined_data.json");
            var json = System.IO.File.ReadAllText(filePath);
            JsonData = JsonConvert.DeserializeObject<dynamic>(json);
        }
    }
}
