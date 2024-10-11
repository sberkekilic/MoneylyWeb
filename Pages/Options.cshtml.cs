using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class OptionsModel : PageModel
{
    public string SelectedOption { get; set; }

    private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");

    public void OnGet()
    {
        if (!System.IO.File.Exists(filePath))
        {
            var defaultData = new SelectionData { SelectedOption = string.Empty };
            var jsonData = JsonSerializer.Serialize(defaultData);
            System.IO.File.WriteAllText(filePath, jsonData);
        }

        var jsonDataFromFile = System.IO.File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<SelectionData>(jsonDataFromFile);
        SelectedOption = data?.SelectedOption;
    }

    [HttpPost]
    public async Task<IActionResult> OnPostSaveSelectionAsync([FromBody] SelectionData data)
    {
        if (data == null || string.IsNullOrEmpty(data.SelectedOption))
        {
            return BadRequest("Invalid data received.");
        }

        var jsonData = JsonSerializer.Serialize(data);
        await System.IO.File.WriteAllTextAsync(filePath, jsonData);

        return new JsonResult(new { success = true });
    }

    public class SelectionData
    {
        public string SelectedOption { get; set; }
    }
}