using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieScheduleApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieScheduleApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        public EditModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Movie Movie { get; set; } = new();
        public void OnGet(int id)
        {
            // ��������� ������ �� JSON
            var movies = LoadMovies();
            Movie = movies.FirstOrDefault(m => m.Id == id) ?? new Movie();
        }

        public IActionResult OnPost(int id, string title, string director, string genre, string description, string sessions)
        {
            Console.WriteLine($"�������� ������: id={id}, title={title}, sessions={sessions}");
            
            Console.WriteLine($"�������������� ������ � ID: {Movie.Id}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("������: ������������ ������ ������.");
                return Page(); // ���� ������ �����������, ���������� �������� ��������������
            }

            var movies = LoadMovies();

            // ������� ����� �� ID
            var existingMovie = movies.FirstOrDefault(m => m.Id == id);
            if (existingMovie == null)
                return BadRequest();
            {
                Console.WriteLine($"������ ����� ��� ��������������: {existingMovie.Title}");
                // ��������� ������ ������
                existingMovie.Title = Movie.Title;
                existingMovie.Director = Movie.Director;
                existingMovie.Genre = Movie.Genre;
                existingMovie.Description = Movie.Description;
                existingMovie.Sessions = Movie.Sessions;

                SaveMovies(movies); // ���������� ���������
                Console.WriteLine("����� ������� ��������.");
            }
            //else
            //{
            //    Console.WriteLine("������: ����� �� ������.");
            //    return NotFound("����� �� ������.");
            //}

            return RedirectToPage("./Index");
        }


        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    // ��������� ������
        //    var movies = LoadMovies();
        //    var existingMovie = movies.FirstOrDefault(m => m.Id == Movies.Id);

        //    if (existingMovie != null)
        //    {
        //        // �������� ������
        //        existingMovie.Title = Movies.Title;
        //        existingMovie.Director = Movies.Director;
        //        existingMovie.Genre = Movies.Genre;
        //        existingMovie.Description = Movies.Description;
        //        existingMovie.Sessions = Movies.Sessions;
        //    }
        //    else
        //    {
        //        // �������� ����� �����, ���� �� �����������
        //        movies.Add(Movies);
        //    }

        //    // ��������� ����������� ������ � JSON
        //    SaveMovies(movies);

        //    // ������������� �� ������� ��������
        //    return RedirectToPage("./Index");
        //}

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
