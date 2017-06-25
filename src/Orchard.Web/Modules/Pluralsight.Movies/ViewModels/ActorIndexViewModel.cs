using Pluralsight.Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Movies.ViewModels
{
    public class ActorsIndexViewModel
    {
        public IEnumerable<ActorRecord> Actors { get; set; }
        public dynamic Pager { get; set; }
    }
}