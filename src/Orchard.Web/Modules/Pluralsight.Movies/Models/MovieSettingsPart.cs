using Orchard.ContentManagement;

namespace Pluralsight.Movies.Models
{
    public class MovieSettingsPart : ContentPart<MovieSettingsPartRecord>
    {
        public string TMDB_APIKey
        {
            get { return Record.TMDB_APIKey; }
            set { Record.TMDB_APIKey = value; }
        }
    }
}