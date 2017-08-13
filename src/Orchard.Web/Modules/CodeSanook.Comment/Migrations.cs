using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CodeSanook.Comment
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("CommentPartRecord", config =>
            config.ContentPartRecord()
            .Column<string>("CommentBody")
            .Column<DateTime>("CreatedUtcDate")
            .Column<DateTime>("LastUpdatedUtcDate"));

            ContentDefinitionManager.AlterTypeDefinition("BlogPost", cfg =>
                cfg.WithPart("CommentContainerPart")
            );

            return 1;
        }
    }
}