using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieScheduleApp.Models;
using System.Text.Json;

namespace MovieScheduleApp.Pages
{
    public class InfoModel : PageModel
    {
        public Movie Movies { get; set; }

        public IActionResult OnGet(int id)
        {
            var jsonPath = Path.Combine("wwwroot", "data", "movies.json");
            var jsonData = System.IO.File.ReadAllText(jsonPath);
            var movies = JsonSerializer.Deserialize<List<Movie>>(jsonData);

            Movies = movies?.FirstOrDefault(m => m.Id == id);
            if (Movies == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
