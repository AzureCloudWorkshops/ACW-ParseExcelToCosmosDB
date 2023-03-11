using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingFunctions
{
    public class MovieToWatch
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Review { get; set; }
        public int Year { get; set; }
    }
}