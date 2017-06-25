using System.Collections.Generic;
using Orchard.ContentManagement;

namespace Pluralsight.Movies.ViewModels {
    public class ActorViewModel {
        public string Name { get; set; }
        public IEnumerable<IContent> Movies { get; set; }
    }
}