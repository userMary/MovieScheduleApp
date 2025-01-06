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
            // Загрузить фильмы из JSON
            var movies = LoadMovies();
            Movie = movies.FirstOrDefault(m => m.Id == id) ?? new Movie();
        }

        public IActionResult OnPost(int id, string title, string director, string genre, string description, string sessions)
        {
            Console.WriteLine($"Получены данные: id={id}, title={title}, sessions={sessions}");
            
            Console.WriteLine($"Редактирование фильма с ID: {Movie.Id}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Ошибка: Некорректная модель данных.");
                return Page(); // Если модель некорректна, возвращаем страницу редактирования
            }

            var movies = LoadMovies();

            // Находим фильм по ID
            var existingMovie = movies.FirstOrDefault(m => m.Id == id);
            if (existingMovie == null)
                return BadRequest();
            {
                Console.WriteLine($"Найден фильм для редактирования: {existingMovie.Title}");
                // Обновляем данные фильма
                existingMovie.Title = Movie.Title;
                existingMovie.Director = Movie.Director;
                existingMovie.Genre = Movie.Genre;
                existingMovie.Description = Movie.Description;
                existingMovie.Sessions = Movie.Sessions;

                SaveMovies(movies); // Сохранение изменений
                Console.WriteLine("Фильм успешно обновлен.");
            }
            //else
            //{
            //    Console.WriteLine("Ошибка: Фильм не найден.");
            //    return NotFound("Фильм не найден.");
            //}

            return RedirectToPage("./Index");
        }


        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    // Загрузить фильмы
        //    var movies = LoadMovies();
        //    var existingMovie = movies.FirstOrDefault(m => m.Id == Movies.Id);

        //    if (existingMovie != null)
        //    {
        //        // Обновить данные
        //        existingMovie.Title = Movies.Title;
        //        existingMovie.Director = Movies.Director;
        //        existingMovie.Genre = Movies.Genre;
        //        existingMovie.Description = Movies.Description;
        //        existingMovie.Sessions = Movies.Sessions;
        //    }
        //    else
        //    {
        //        // Добавить новый фильм, если он отсутствует
        //        movies.Add(Movies);
        //    }

        //    // Сохранить обновленный список в JSON
        //    SaveMovies(movies);

        //    // Перенаправить на главную страницу
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
            // Сериализация данных в JSON
            var json = JsonConvert.SerializeObject(movies, Formatting.Indented);

            // Вывод данных в консоль перед записью
            Console.WriteLine("Запись данных в файл:");
            Console.WriteLine(json);

            // Записываем данные в файл
            System.IO.File.WriteAllText(filePath, json);

            // Подтверждение успешного сохранения
            Console.WriteLine("Данные успешно записаны в файл: " + filePath);
        }
    }
}
