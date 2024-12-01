using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieScheduleApp.Models;
using System.Text.Json;

namespace MovieScheduleApp.Pages
{
    public class InfoModel : PageModel
    {
        public Movie Movie { get; set; }

        public IActionResult OnGet(int id)
        {
            var jsonPath = Path.Combine("wwwroot", "data", "movies.json");
            var jsonData = System.IO.File.ReadAllText(jsonPath);
            var movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);

            Movie = movies?.FirstOrDefault(m => m.Id == id);
            if (Movie == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
