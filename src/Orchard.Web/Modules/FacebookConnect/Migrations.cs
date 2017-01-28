using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace FacebookConnect {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("FacebookSettingsPartRecord",
                                      table => table
                                          .ContentPartRecord()
                                          .Column<string>("AppId", c => c.Unlimited())
                                          .Column<string>("AppSecret", c => c.Unlimited())
                                          .Column<string>("Permissions", c => c.Unlimited())
                );

            SchemaBuilder.CreateTable("FacebookUserPartRecord",
                                      table => table
                                          .ContentPartRecord()
                                          .Column<string>("UserId")
                );

            ContentDefinitionManager.AlterTypeDefinition("FacebookConnectWidget",
                cfg => cfg
                    .WithPart("FacebookConnectPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            ContentDefinitionManager.AlterPartDefinition("FacebookConnectPart", builder => builder.Attachable());

            return 1;
        }

    }
}