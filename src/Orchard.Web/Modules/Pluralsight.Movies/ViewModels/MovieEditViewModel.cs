using Pluralsight.Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Movies.ViewModels
{
    public class MovieEditViewModel
    {
        public string IMDB_Id { get; set; }
        public int YearReleased { get; set; }
        public MPAARating Rating { get; set; } 

        public IEnumerable<int> Actors { get; set; } 
        public IEnumerable<ActorRecord> AllActors { get; set; } 
    }
}