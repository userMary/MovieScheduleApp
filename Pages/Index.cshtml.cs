using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieScheduleApp.Models;
using System.Text.Json;

namespace MovieScheduleApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Movie> Movies { get; set; } = new();

        public void OnGet()
        {
            var jsonPath = Path.Combine("wwwroot", "data", "movies.json");
            var jsonData = System.IO.File.ReadAllText(jsonPath);
            Movies = JsonSerializer.Deserialize<List<Movie>>(jsonData) ?? new List<Movie>();
        }
    }
}
