namespace Pluralsight.Movies.Models
{
    public class MovieActorRecord
    {
        public virtual int Id { get; set; }
        public virtual MoviePartRecord MoviePartRecord { get; set; }
        public virtual ActorRecord ActorRecord { get; set; }
    }
}