using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Pluralsight.Movies.Models
{
    public class MoviePart:ContentPart<MoviePartRecord>
    {
        public virtual string IMDB_Id
        {
            get { return Record.IMDB_Id; }
            set { Record.IMDB_Id = value;}
        }

        public virtual int YearReleased
        {
            get { return Record.YearReleased; }
            set { Record.YearReleased = value; }
        }

        public virtual MPAARating Rating
        {
            get { return Record.Rating; }
            set { Record.Rating = value; }
        }
    }
}