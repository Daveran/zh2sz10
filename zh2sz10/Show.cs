using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zh2sz10
{
    internal class Show
    {
        public Guid Id { get; set; }
        public int show_id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string director { get; set; }
        public string cast { get; set; }
        public string country { get; set; }
        public string date_added { get; set; }
        public int release_year { get; set; }
        public string rating { get; set; }
        public string duration { get; set; }
        public string listed_in { get; set; }
        public string description { get; set; }

        public Show(int show_id, string type, string title, string director, string cast, 
            string country, string date_added, 
            int release_year, string rating, string duration, string listed_in, string description)
        {
            this.show_id = show_id;
            this.type = type;
            this.title = title;
            this.director = director;
            this.cast = cast;
            this.country = country;
            this.date_added = date_added;
            this.release_year = release_year;
            this.rating = rating;
            this.duration = duration;
            this.listed_in = listed_in;
            this.description = description;
        }

        
    }
}
