using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using CodeSanook.FacebookPage.Models;

namespace CodeSanook.FacebookPage
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            // Creating table FacebookPageRecord 
            SchemaBuilder.CreateTable(typeof(FacebookPagePartRecord).Name, table => table
                .ContentPartRecord()
                .Column<string>("Href")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(FacebookPagePart).Name, cfg => cfg.Attachable()
            );

            ContentDefinitionManager.AlterTypeDefinition("FacebookPageWidget", cfg => cfg
                .WithPart(typeof(FacebookPagePart).Name)
                .AsWidget()
            );//this require reference to Orchard.Widget 

            return 1;
        }
    }
}