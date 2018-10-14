using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System;

namespace Orchard.Users {
    public class UsersDataMigration : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("UserPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("UserName")
                    .Column<string>("Email")
                    .Column<string>("NormalizedUserName")
                    .Column<string>("Password")
                    .Column<string>("PasswordFormat")
                    .Column<string>("HashAlgorithm")
                    .Column<string>("PasswordSalt")
                    .Column<string>("RegistrationStatus", c => c.WithDefault("Approved"))
                    .Column<string>("EmailStatus", c => c.WithDefault("Approved"))
                    .Column<string>("EmailChallengeToken")
                    .Column<DateTime>("CreatedUtc")
                    .Column<DateTime>("LastLoginUtc")
                    .Column<DateTime>("LastLogoutUtc")
                    .Column<DateTime>("LastPasswordChangeUtc", c => c.WithDefault(new DateTime(1990, 1, 1)))
                );

            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg.Creatable(false));

            return 1;
        }
    }
}