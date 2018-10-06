using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pluralsight.Movies.Models;

namespace Pluralsight.Movies.ViewModels {
    public class EditActorViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "The actor's name is required")]
        public string Name { get; set; }

        public IEnumerable<MoviePart> Movies { get; set; }
    }
}