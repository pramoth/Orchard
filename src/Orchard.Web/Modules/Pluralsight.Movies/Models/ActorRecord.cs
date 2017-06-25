using System.Collections.Generic;

namespace Pluralsight.Movies.Models {
    public class ActorRecord {
        public virtual int Id { get; set; } 
        public virtual string Name { get; set; }

        public ActorRecord() {
            ActorMovies = new List<MovieActorRecord>();
        }

        public virtual IList<MovieActorRecord> ActorMovies { get; set; }
    }
}