using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieScheduleApp.Models;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieScheduleApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Movie> Movies { get; set; } = new();
        public List<Movie> FilteredMovies { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public void OnGet()
        {
            LoadMovies();
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                FilteredMovies = Movies.Where(m =>
                    m.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Director.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Genre.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Sessions.Any(s => s.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            else
            {
                FilteredMovies = Movies;
            }
        }

        private void LoadMovies()
        {
            var jsonPath = Path.Combine("wwwroot", "data", "movies.json");
            if (System.IO.File.Exists(jsonPath))
            {
                var jsonData = System.IO.File.ReadAllText(jsonPath);
                Movies = JsonConvert.DeserializeObject<List<Movie>>(jsonData) ?? new List<Movie>(); // JsonConvert.DeserializeObject // JsonSerializer.Deserialize
            }
        }


    }
}

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Sessions { get; set; } = new();
}
