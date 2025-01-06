using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieScheduleApp.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public DeleteModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Movie Movie { get; set; } = new();
        public void OnGet(int id)
        {
            var movies = LoadMovies();
            Movie = movies.FirstOrDefault(m => m.Id == id) ?? new Movie();
        }

        public IActionResult OnPost(int id)
        {
            Console.WriteLine($"�������� ������ � ID: {id}");

            if (id == 0)
            {
                Console.WriteLine("������: ID ������ �����������.");
                return BadRequest("ID ������ �����������.");
            }
            // ��������� ������ �� �����
            var movies = LoadMovies();

            // ���� ����� �� ID
            var movieToRemove = movies.FirstOrDefault(m => m.Id == id);
            if (movieToRemove == null)
                return BadRequest();
            {
                Console.WriteLine($"������ ����� ��� ��������: {movieToRemove.Title}");
                movies.Remove(movieToRemove); // ������� ����� �� ������
                // ��������� ��������� � JSON-����
                SaveMovies(movies);
                Console.WriteLine("����� ������� ������.");
            }
            //else
            //{
            //    Console.WriteLine("������: ����� �� ������.");
            //    return NotFound("����� �� ������.");
            //}

            return RedirectToPage("./Index");
        }

        private List<Movie> LoadMovies()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "data", "movies.json");

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
            }

            return new List<Movie>();
        }

        private void SaveMovies(List<Movie> movies)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "data", "movies.json");
            // ������������ ������ � JSON
            var json = JsonConvert.SerializeObject(movies, Formatting.Indented);

            // ����� ������ � ������� ����� �������
            Console.WriteLine("������ ������ � ����:");
            Console.WriteLine(json);

            // ���������� ������ � ����
            System.IO.File.WriteAllText(filePath, json);

            // ������������� ��������� ����������
            Console.WriteLine("������ ������� �������� � ����: " + filePath);
        }
    }
}
