using Orchard.ContentManagement.Records;

namespace Pluralsight.Movies.Models
{
    public class MovieSettingsPartRecord : ContentPartRecord
    {
        public virtual string TMDB_APIKey { get; set; }
    }
}