using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using FacebookConnect.Models;

namespace FacebookConnect
{
    public class Migrations : DataMigrationImpl {

        public int Create() {

            //create table for a setting record 
            SchemaBuilder.CreateTable( nameof(FacebookSettingsPartRecord),
                                      table => table
                                          .ContentPartRecord()
                                          .Column<string>("AppId", c => c.Unlimited())
                                          .Column<string>("AppSecret", c => c.Unlimited()));

            //create a table for FacebookUserPart record
            SchemaBuilder.CreateTable( nameof(FacebookUserPartRecord),
                                      table => table
                                          .ContentPartRecord()
                                          .Column<string>("FirstName")
                                          .Column<string>("LastName")
                                          .Column<string>("ProfilePictureUrl")
                );

            //alter User content type to attach FacebookUserPart
            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg .WithPart("FacebookUserPart"));

            return 1;
        }

    }
}