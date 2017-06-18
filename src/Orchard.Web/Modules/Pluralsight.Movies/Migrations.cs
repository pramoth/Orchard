using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Newtonsoft.Json;

namespace Pluralsight.Movies
{
    public class Migrations : DataMigrationImpl
    {

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
            var urlPatterns = new []
            {
                new {Name = "Movie Title", Pattern = "movies/{Content.Slug}", Description = "movies/movie-title"},
                new {Name = "Film Title", Pattern = "films/{Content.Slug}", Description = "films/film-title"},
            };
            
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder =>
             builder
             .WithPart("BodyPart", partBuilder => 
                partBuilder.WithSetting("BodyTypePartSettings.Flavor","text"))
             .WithPart("AutoroutePart", partBuilder =>
                partBuilder
                .WithSetting("AutorouteSettings.AllowCustomPattern","true")
                .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit","false")
                .WithSetting("AutorouteSettings.PatternDefinitions", JsonConvert.SerializeObject(urlPatterns))
                .WithSetting("AutorouteSettings.DefaultPatternIndex ","0"))
            );

            return 3;
        }

        public int UpdateFrom3() {
            SchemaBuilder.CreateTable("MoviePartRecord", table =>
                table.ContentPartRecord()
                    .Column<string>("IMDB_Id")
                    .Column<int>("YearReleased")
                    .Column<string>("Rating",col=>col.WithLength(4)));

            ContentDefinitionManager.AlterTypeDefinition("Movie",
                builder => builder.WithPart("MoviePart"));

            return 4;
        }
    }
}