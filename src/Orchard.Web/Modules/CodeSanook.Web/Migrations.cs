using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Modules.Services;
using System.Linq;
using Orchard.Modules.Models;
using Orchard;
using Orchard.Users.Models;
using Orchard.Security;
using Orchard.ContentManagement;
using Orchard.Roles.Models;
using Orchard.Data;
using Orchard.Roles.Services;
using Orchard.Themes.Services;

namespace CodeSanook.Web
{
    public class Migrations : DataMigrationImpl
    {

        private readonly IModuleService moduleService;
        private readonly IOrchardServices orchardService;
        private Dictionary<string, ModuleFeature> features;
        private readonly IRepository<UserRolesPartRecord> userRolesRepository;
        private readonly IRoleService roleService;
        private readonly IThemeService themeService;
        private readonly ISiteThemeService siteThemeService;

        public Migrations(
            IModuleService moduleService,
            IOrchardServices orchardService,
            IRepository<UserRolesPartRecord> userRolesRepository,
            IRoleService roleService,
            IThemeService themeService,
            ISiteThemeService siteThemeService)
        {
            this.moduleService = moduleService;
            this.orchardService = orchardService;
            this.userRolesRepository = userRolesRepository;
            this.roleService = roleService;
            this.themeService = themeService;
            this.siteThemeService = siteThemeService;
            features = moduleService.GetAvailableFeatures().ToDictionary(m => m.Descriptor.Id, m => m);
        }

        public int Create()
        {
            //assign admin role
            var admin = orchardService.ContentManager.Query<UserPart, UserPartRecord>()
               .Where<UserPartRecord>(x => x.UserName == "admin")
               .List<IUser>()
               .SingleOrDefault();

            var roles = admin.As<UserRolesPart>().Roles;
            var adminRole = roleService.GetRoleByName("Administrator");
            if (!roles.Any(r => r == adminRole.Name))
            {
                //add role
                var userRolesPartRecord = new UserRolesPartRecord()
                {
                    UserId = admin.Id,
                    Role = adminRole,
                };

                userRolesRepository.Create(userRolesPartRecord);
            }

            EnableModules(new[] { "CodeSanook.AmazonS3", "Markdown", "CodeSanook.FacebookConnect" });
            DisableModules(new[] { "Comments" });

            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
                cfg => cfg
                    .WithPart("BodyPart", partBuilder => partBuilder
                    //body part use this type to get a setting
                        .WithSetting("BodyTypePartSettings.Flavor", "markdown"))
                     //.RemovePart("CommentsContainerPart")
                     .RemovePart("CommentsPart") //defined in blog.recipe.xml
            );

            ContentDefinitionManager.AlterTypeDefinition("Blog",
                cfg => cfg.RemovePart("CommentsContainerPart"));

            //update theme
            var themeId = "TheBootstrapMachine";
            themeService.EnableThemeFeatures(themeId);
            siteThemeService.SetSiteTheme(themeId);

            return 1;
        }

        private void EnableModules(string[] modulesToEnable)
        {
            foreach (var module in modulesToEnable)
            {
                if (features.ContainsKey(module) && !features[module].IsEnabled)
                {
                    moduleService.EnableFeatures(new string[] { module });
                }
            }
        }

        private void DisableModules(string[] modulesToDisable)
        {
            foreach (var module in modulesToDisable)
            {
                if (features.ContainsKey(module) && !features[module].IsEnabled)
                {
                    moduleService.DisableFeatures(new string[] { module });
                }
            }
        }

    }
}