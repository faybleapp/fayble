using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fayble.Models.Metadata
{

    public class SeriesResult
    {
        public string Id { get;  }
        public string Name { get;  }
        public string Summary { get;  }
        public int StartYear { get; }
        public List<BookResult> books { get; }

        public SeriesResult(string id, string name, int startYear, string summary, List<BookResult> books)
        {
            this.books = books;
            Id = id;
            Name = name;
            StartYear = startYear;
            Summary = summary;
        }
    }


}
