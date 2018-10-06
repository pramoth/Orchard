using System;
using Orchard.Data.Migration;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;

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
                    .Column<int>("ContentItemId ")
                    );


            //create Comment type
            ContentDefinitionManager.AlterTypeDefinition("Comment",
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("CommentPart")
                    .RemovePart("IdentityPart")
                    .Listable()
                    );

            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
                cfg => cfg.WithPart("CommentContainerPart")
            );

            return 1;
        }
    }
}