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
            Console.WriteLine($"Удаление фильма с ID: {id}");

            if (id == 0)
            {
                Console.WriteLine("Ошибка: ID фильма отсутствует.");
                return BadRequest("ID фильма отсутствует.");
            }
            // Загружаем фильмы из файла
            var movies = LoadMovies();

            // Ищем фильм по ID
            var movieToRemove = movies.FirstOrDefault(m => m.Id == id);
            if (movieToRemove == null)
                return BadRequest();
            {
                Console.WriteLine($"Найден фильм для удаления: {movieToRemove.Title}");
                movies.Remove(movieToRemove); // Удаляем фильм из списка
                // Сохраняем изменения в JSON-файл
                SaveMovies(movies);
                Console.WriteLine("Фильм успешно удален.");
            }
            //else
            //{
            //    Console.WriteLine("Ошибка: Фильм не найден.");
            //    return NotFound("Фильм не найден.");
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
