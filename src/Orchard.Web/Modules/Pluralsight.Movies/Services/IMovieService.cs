using Orchard;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pluralsight.Movies.Models;
using Pluralsight.Movies.ViewModels;

namespace Pluralsight.Movies.Services
{
    public interface IMovieService : IDependency
    {
        void UpdateMovie(MovieEditViewModel viewModel, MoviePart part);
    }
}