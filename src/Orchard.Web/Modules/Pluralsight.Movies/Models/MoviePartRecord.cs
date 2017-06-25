using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Pluralsight.Movies.Models
{
    public class MoviePartRecord:ContentPartRecord
    {
        public virtual string IMDB_Id { get; set; }
        public virtual int YearReleased { get; set; }
        public virtual MPAARating Rating { get; set; }

        public MoviePartRecord() {
            MovieActors = new List<MovieActorRecord>();
        }

        public virtual IList<MovieActorRecord> MovieActors { get; set; }
    }
}