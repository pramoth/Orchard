using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Pluralsight.Movies.Models;
using Pluralsight.Movies.ViewModels;
using Orchard.Data;
using Pluralsight.Movies.Services;

namespace Pluralsight.Movies.Drivers
{
    public class MoviePartDriver : ContentPartDriver<MoviePart>
    {
        private readonly IRepository<ActorRecord> actorRepository;
        private readonly IMovieService movieService;

        protected override string Prefix => "Movie";

        public MoviePartDriver(IRepository<ActorRecord> actorRepository, 
            IMovieService movieService)
        {
            this.actorRepository = actorRepository;
            this.movieService = movieService;
        }
        protected override DriverResult Display(MoviePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Movie", () => shapeHelper.Parts_Movie(MoviePart: part));
        }

        protected override DriverResult Editor(MoviePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Movie_Edit", () =>
            shapeHelper.EditorTemplate(TemplateName: "Parts/Movie", 
            Model: BuildEditorViewModel(part), Prefix: this.Prefix));

        }

        protected override DriverResult Editor(MoviePart part, IUpdateModel updater, dynamic shapeHelper) {
            //update form input to view model
            var viewModel = new MovieEditViewModel();
            updater.TryUpdateModel(viewModel, Prefix, null, new[] { "AllActors" });
            movieService.UpdateMovie(viewModel, part);
            return Editor(part, updater);
        }

        private MovieEditViewModel BuildEditorViewModel(MoviePart part)
        {
            return new MovieEditViewModel()
            {
                IMDB_Id = part.IMDB_Id,
                Actors = part.Cast.Select(c => c.Id).ToList(),
                AllActors = actorRepository.Table.OrderBy(a => a.Name).ToList(),
                Rating = part.Rating,
                YearReleased = part.YearReleased,
            };
        }
    }
}