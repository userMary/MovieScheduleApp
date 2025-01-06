using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieScheduleApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Sessions { get; set; } = new();

        // Виртуальное свойство для работы с сеансами в виде строки
        [JsonIgnore]
        public string SessionsAsString
        {
            get => string.Join(", ", Sessions);
            set => Sessions = value.Split(',').Select(s => s.Trim()).ToList();
        }
    }
}
