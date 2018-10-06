using System;
using Pluralsight.Movies.Models;
using Pluralsight.Movies.ViewModels;
using Orchard.Data;
using System.Linq;

namespace Pluralsight.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<ActorRecord> actorRepository;
        private readonly IRepository<MovieActorRecord> movieActorRepository;

        public MovieService(IRepository<ActorRecord> actorRepository,
            IRepository<MovieActorRecord> movieActorRepository)
        {
            this.actorRepository = actorRepository;
            this.movieActorRepository = movieActorRepository;
        }

        public void UpdateMovie(MovieEditViewModel viewModel, MoviePart part)
        {
            //now form data is in view model
            //save part
            part.IMDB_Id = viewModel.IMDB_Id;
            part.YearReleased = viewModel.YearReleased;
            part.Rating = viewModel.Rating;

            var oldCast = movieActorRepository
                .Fetch(ma => ma.MoviePartRecord.Id == part.Id)
                .Select(r => r.ActorRecord.Id).ToList();

            foreach (var oldActorId in oldCast.Except(viewModel.Actors))
            {
                movieActorRepository
                    .Delete(movieActorRepository.Get(r => r.ActorRecord.Id == oldActorId));
            }

            //create link table
            foreach (var newActorId in viewModel.Actors.Except(oldCast))
            {
                var actor = actorRepository.Get(newActorId);
                movieActorRepository.Create(
                    new MovieActorRecord
                    {
                        ActorRecord = actor,
                        MoviePartRecord = part.Record
                    });
            }

        }
    }
}