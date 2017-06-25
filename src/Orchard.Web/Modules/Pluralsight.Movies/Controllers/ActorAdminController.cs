using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Pluralsight.Movies.Models;
using Pluralsight.Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pluralsight.Movies.Controllers
{
    public class ActorsAdminController:Controller
    {
        private readonly IOrchardServices orchardServices;
        private readonly ISiteService siteService;
        private readonly IRepository<ActorRecord> actorRepository;
        private readonly dynamic shape;
        
        public Localizer T { get; set; }

        public ActorsAdminController(
            IOrchardServices orchardServices,
            ISiteService siteService,
            IRepository<ActorRecord> actorRepository,
            IShapeFactory shapeFactory) {
            this.orchardServices = orchardServices;
            this.siteService = siteService;
            this.actorRepository = actorRepository;
            this.shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        [Admin]
        public ActionResult Index(PagerParameters pagerParameters )
        {
            var pager = new Pager(siteService.GetSiteSettings(), pagerParameters);

            var actorCount = actorRepository.Table.Count();
            var actors = actorRepository.Table
                .OrderBy(a => a.Name)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToList();
            var pagerShape = shape.Pager(pager)
                .TotalItemCount(actorCount); //set property of Pager shape

            var viewModel = new ActorsIndexViewModel { Actors = actors, Pager = pagerShape };
            return View(viewModel);
        }

        [HttpGet, Admin]
        public ActionResult Create() {
            return View(new CreateActorViewModel());
        }

        [HttpPost, ActionName("Create"), Admin]
        public ActionResult CreatePOST(CreateActorViewModel viewModel) {

            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            actorRepository.Create(new ActorRecord { Name = viewModel.Name });
            orchardServices.Notifier.Add(NotifyType.Information, T("Created the actor {0}", viewModel.Name));
            return RedirectToAction("Index");
        }
        
        [HttpGet, Admin]
        public ActionResult Edit(int actorId) {
            var actor = actorRepository.Get(actorId);
            if (actor == null) {
                return new HttpNotFoundResult("Could not find the actor with id " + actorId);
            }
            var actorMovies = orchardServices.ContentManager.GetMany<MoviePart>(
                actor.ActorMovies.Select(m => m.MoviePartRecord.Id), 
                VersionOptions.Published, QueryHints.Empty);
            return View(new EditActorViewModel { Id = actorId, Name = actor.Name, Movies = actorMovies });
        }

        [HttpPost, ActionName("Edit"), Admin]
        public ActionResult EditPOST(EditActorViewModel viewModel) {
            var actor = actorRepository.Get(viewModel.Id);

            if (!ModelState.IsValid) {
                viewModel.Movies = orchardServices.ContentManager.GetMany<MoviePart>(actor.ActorMovies.Select(m => m.MoviePartRecord.Id), VersionOptions.Published, QueryHints.Empty);
                return View("Edit", viewModel);
            }
            
            actor.Name = viewModel.Name;
            actorRepository.Update(actor);
            orchardServices.Notifier.Add(NotifyType.Information, T("Saved {0}", viewModel.Name));
            return RedirectToAction("Index");
        }

        [HttpGet, Admin]
        public ActionResult Delete(int actorId) {
            var actor = actorRepository.Get(actorId);
            if (actor == null) {
                return new HttpNotFoundResult("Could not find the actor with id " + actorId);
            }
            actorRepository.Delete(actor);
            orchardServices.Notifier.Add(NotifyType.Information, T("The actor {0} has been deleted", actor.Name));
            return RedirectToAction("Index");
        }

    }
}