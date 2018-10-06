﻿using Orchard.ContentManagement;
using Orchard.Environment;
using Orchard.Environment.Extensions.Models;
using Orchard.Logging;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Pluralsight.Movies.Handlers
{
    public class MovieFeatureEventHandler : IFeatureEventHandler
    {
        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;

        public ILogger Logger { get; set; }

        public MovieFeatureEventHandler(ITaxonomyService taxonomyService,
            IContentManager contentManager)
        {
            _taxonomyService = taxonomyService;
            _contentManager = contentManager;
            Logger = NullLogger.Instance;
        }

        public void Installing(Feature feature)
        {
        }

        public void Installed(Feature feature)
        {
        }

        public void Enabling(Feature feature)
        {
        }

        public void Enabled(Feature feature)
        {
            Logger.Information("movie feature enabled");
            if (_taxonomyService.GetTaxonomyByName("Genre") == null)
            {
                Logger.Information("creating Genre taxonomy");
                //create Taxonomy content type as cast to Taxonomy part
                var genre = _contentManager.New<TaxonomyPart>("Taxonomy");
                genre.Name = "Genre";
                _contentManager.Create(genre, VersionOptions.Published);
                CreateTerm(genre, "Action");
                CreateTerm(genre, "Adventure");
                CreateTerm(genre, "Animation");
                CreateTerm(genre, "Comedy");
                CreateTerm(genre, "Crime");
                CreateTerm(genre, "Documentation");
                CreateTerm(genre, "Drama");

                Logger.Information("Genre taxonomy get created");
            }
        }

        private void CreateTerm(TaxonomyPart genre, string genreName)
        {
            var term = _taxonomyService.NewTerm(genre);
            term.Name = genreName;
            _contentManager.Create(term, VersionOptions.Published);
        }

        public void Disabling(Feature feature)
        {
        }

        public void Disabled(Feature feature)
        {
        }

        public void Uninstalling(Feature feature)
        {
        }

        public void Uninstalled(Feature feature)
        {
        }
    }
}