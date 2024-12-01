using System.Collections.Generic;

namespace MovieScheduleApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public List<string> Sessions { get; set; }
    }
}
