using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Themes;
using Pluralsight.Movies.Models;
using Pluralsight.Movies.ViewModels;

namespace Pluralsight.Movies.Controllers {
    [Themed]
    public class ActorController : Controller {
        private readonly IRepository<ActorRecord> actorRepository;
        private readonly IContentManager contentManager;

        public ActorController(IRepository<ActorRecord> actorRepository, IContentManager contentManager) {
            this.actorRepository = actorRepository;
            this.contentManager = contentManager;
        }

        [HttpGet]
        public ActionResult Details(int actorId) {
            return ActorDetails(() => actorRepository.Get(actorId));
        }

        [HttpGet]
        public ActionResult DetailsByName(string actorName) {
            return ActorDetails(() => actorRepository.Fetch(a => a.Name == actorName).SingleOrDefault());
        }

        private ActionResult ActorDetails(Func<ActorRecord> getActor) {
            var actor = getActor();
            if (actor == null) {
                return HttpNotFound();
            }

            var movieIds = actor.ActorMovies.Select(m => m.MoviePartRecord.Id);
            var movies = contentManager.GetMany<MoviePart>(movieIds, VersionOptions.Published, QueryHints.Empty)
                .OrderByDescending(m => m.YearReleased).ToList();

            var viewModel = new ActorViewModel {Name = actor.Name, Movies = movies};
            return View("Details", viewModel);
        }
    }
}