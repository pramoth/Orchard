using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using FacebookConnect.Models;

namespace FacebookConnect {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            //create table for a setting record 
            SchemaBuilder.CreateTable( nameof(FacebookSettingsPartRecord),
                                      table => table
                                          .ContentPartRecord()
                                          .Column<string>("AppId", c => c.Unlimited())
                                          .Column<string>("AppSecret", c => c.Unlimited())
                                          .Column<string>("Permissions", c => c.Unlimited())
                );

            //create a table for FacebookUserPart record
            SchemaBuilder.CreateTable( nameof(FacebookUserPartRecord),
                                      table => table
                                          .ContentPartRecord()
                                          .Column<int>("UserId")
                );

            ContentDefinitionManager.AlterPartDefinition( nameof(FacebookConnectPart), builder => builder.Attachable());

            //create Widget content type
            ContentDefinitionManager.AlterTypeDefinition("FacebookConnectWidget",
                cfg => cfg
                    .WithPart(nameof(FacebookConnectPart))
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }

    }
}