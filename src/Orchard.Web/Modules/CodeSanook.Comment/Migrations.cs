using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace CodeSanook.Comment
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("CommentPartRecord",
                config => config
                    .ContentPartRecord()
                    .Column<string>("CommentBody")
                    .Column<DateTime>("CreatedUtcDate")
                    .Column<DateTime>("LastUpdatedUtcDate")
                    .Column<int>("ContentItemId "));

            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
                cfg => cfg.WithPart("CommentContainerPart")
            );

            //create Comment type
            ContentDefinitionManager.AlterTypeDefinition("Comment",
                cfg => cfg
                    .WithPart("CommentPart")
                    .WithPart("CommonPart")
                    .RemovePart("IdentityPart"));
            return 1;
        }
    }
}