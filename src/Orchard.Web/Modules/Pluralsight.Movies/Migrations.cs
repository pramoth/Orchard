using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Newtonsoft.Json;
using Orchard.Data;
using Pluralsight.Movies.Models;

namespace Pluralsight.Movies
{
    public class Migrations : DataMigrationImpl
    {
        private IRepository<ActorRecord> actorRepository;

        public Migrations(IRepository<ActorRecord> actorRepository)
        {
            this.actorRepository = actorRepository;
        }
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder =>
             builder.WithPart("CommonPart")
             .WithPart("TitlePart")
             .WithPart("AutoroutePart")
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder =>
             builder.WithPart("BodyPart")
             .Creatable()
             .Draftable()
            );

            return 2;
        }

        //alter part setting
        public int UpdateFrom2()
        {
            var urlPatterns = new[]
            {
                new {Name = "Movie Title", Pattern = "movies/{Content.Slug}", Description = "movies/movie-title"},
                new {Name = "Film Title", Pattern = "films/{Content.Slug}", Description = "films/film-title"},
            };

            ContentDefinitionManager.AlterTypeDefinition("Movie", builder =>
             builder
             .WithPart("BodyPart", partBuilder =>
                partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
             .WithPart("AutoroutePart", partBuilder =>
                partBuilder
                .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                .WithSetting("AutorouteSettings.PatternDefinitions", JsonConvert.SerializeObject(urlPatterns))
                .WithSetting("AutorouteSettings.DefaultPatternIndex ", "0"))
            );

            return 3;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.CreateTable("MoviePartRecord", table =>
                table.ContentPartRecord()
                    .Column<string>("IMDB_Id")
                    .Column<int>("YearReleased")
                    .Column<string>("Rating", col => col.WithLength(4)));

            ContentDefinitionManager.AlterTypeDefinition("Movie",
                builder => builder.WithPart("MoviePart"));

            return 4;
        }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterPartDefinition("MoviePart", builder =>
                builder.WithField("Genre", cfg =>
                    cfg.OfType("TaxonomyField")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "Genre")
                        .WithSetting("TaxonomyFieldSettings.LeavesOnly", "false")
                        .WithSetting("TaxonomyFieldSettings.SingleChoice", "false")
                        .WithSetting("TaxonomyFieldSettings.Hint",
                        "Select many genres as you want.")));

            return 5;
        }

        public int UpdateFrom5()
        {
            SchemaBuilder.CreateTable("ActorRecord", table =>
            {
                table
                .Column<int>("Id", col => col.PrimaryKey().Identity())
                .Column<string>("Name");
            });

            SchemaBuilder.CreateTable("MovieActorRecord", table =>
            {
                table
                .Column<int>("Id", col => col.PrimaryKey().Identity())
                .Column<int>("MoviePartRecord_Id")
                .Column<int>("ActorRecord_Id");
            });

            return 6;
        }

        public int UpdateFrom6()
        {
            actorRepository.Create(new ActorRecord() { Name = "Actor 1" });
            actorRepository.Create(new ActorRecord() { Name = "Actor 2" });
            actorRepository.Create(new ActorRecord() { Name = "Actor 3" });
            actorRepository.Create(new ActorRecord() { Name = "Actor 4" });
            actorRepository.Create(new ActorRecord() { Name = "Actor 5" });
            actorRepository.Create(new ActorRecord() { Name = "Actor 6" });
            return 7;
        }
        
    }
}