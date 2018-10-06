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

            EnableModules(new[]
            {
                "CodeSanook.Configuration",
                "CodeSanook.AmazonS3",
                "Markdown",
                "CodeSanook.FacebookConnect",
                "CodeSanook.FacebookPage",
                "CodeSanook.Comment"
            });
            DisableModules(new[]
            {
                "Orchard.Comments",
            });

            ContentDefinitionManager.AlterTypeDefinition("BlogPost",
                cfg => cfg
                    .WithPart("BodyPart", partBuilder => partBuilder
                    //body part use this type to get a setting
                        .WithSetting("BodyTypePartSettings.Flavor", "markdown"))
                    .RemovePart("CommentsContainerPart")
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
                if (features.ContainsKey(module) && features[module].IsEnabled)
                {
                    moduleService.DisableFeatures(new string[] { module });
                }
            }
        }
    }
}
/*
all built-in modules
Contrib.Profile
Lucene
Markdown
Orchard.Alias
Orchard.Alias.UI
Orchard.Alias.Updater
Orchard.Alias.BreadcrumbLink
Orchard.AntiSpam
Akismet.Filter
TypePad.Filter
Orchard.ArchiveLater
Orchard.AuditTrail
Orchard.AuditTrail.ImportExport
Orchard.AuditTrail.Trimming
Orchard.AuditTrail.Users
Orchard.AuditTrail.Roles
Orchard.AuditTrail.ContentDefinition
Orchard.AuditTrail.RecycleBin
Orchard.Autoroute
Orchard.Azure
Orchard.Azure.Media
Orchard.Azure.OutputCache
Orchard.Azure.DatabaseCache
Orchard.Azure.MediaServices
Orchard.Blogs
Orchard.Blogs.RemotePublishing
Orchard.Caching
Orchard.CodeGeneration
Orchard.Comments
Orchard.Comments.Workflows
Orchard.Conditions
Orchard.ContentPermissions
Orchard.ContentPicker
Orchard.ContentPicker.LocalizationExtensions
Orchard.ContentTypes
Orchard.CustomForms
Orchard.Dashboards
Orchard.DesignerTools
UrlAlternates
WidgetAlternates
Orchard.DynamicForms
Orchard.DynamicForms.AntiSpam
Orchard.DynamicForms.Taxonomies
Orchard.DynamicForms.Projections
Orchard.DynamicForms.Activities.Validation
Orchard.DynamicForms.Bindings.Users
Orchard.Email
Orchard.Email.Workflows
Orchard.Fields
Orchard.Forms
Orchard.ImageEditor
Orchard.ImportExport
Orchard.Indexing
Orchard.JobsQueue
Orchard.JobsQueue.UI
Orchard.jQuery
Orchard.Layouts
Orchard.Layouts.Snippets
Orchard.Layouts.Markdown
Orchard.Layouts.Projections
Orchard.Layouts.Tokens
Orchard.Layouts.UI
Orchard.Lists
Orchard.Localization
Orchard.Localization.DateTimeFormat
Orchard.Localization.CultureSelector
Orchard.Localization.Transliteration
Orchard.Localization.Transliteration.SlugGeneration
Orchard.Media
Orchard.MediaLibrary
Orchard.MediaPicker
Orchard.MediaProcessing
Orchard.MessageBus
Orchard.MessageBus.DistributedSignals
Orchard.MessageBus.SqlServerServiceBroker
Orchard.MessageBus.DistributedShellRestart
Orchard.Migrations
DatabaseUpdate
Orchard.Modules
Orchard.MultiTenancy
Orchard.OutputCache
Orchard.OutputCache.Database
Orchard.OutputCache.FileSystem
Orchard.Packaging
PackagingServices
Gallery
Gallery.Updates
Orchard.Pages
Orchard.Projections
Orchard.PublishLater
Orchard.Recipes
Orchard.Redis
Orchard.Redis.MessageBus
Orchard.Redis.OutputCache
Orchard.Redis.Caching
Orchard.Resources
Orchard.Roles
Orchard.Roles.Workflows
Orchard.Rules
Orchard.Scripting
Orchard.Scripting.Lightweight
Orchard.Scripting.Rules
Orchard.Scripting.CSharp
Orchard.Scripting.CSharp.Validation
Orchard.Scripting.Dlr
Orchard.Search
Orchard.Search.Content
Orchard.Search.ContentPicker
Orchard.Search.MediaLibrary
Orchard.SecureSocketsLayer
Orchard.Setup
Orchard.Setup.Services
Orchard.Tags
Orchard.Tags.Feeds
Orchard.Tags.TagCloud
Orchard.TaskLease
Orchard.Taxonomies
Orchard.Templates
Orchard.Templates.Razor
Orchard.Themes
Orchard.Tokens
Orchard.Tokens.Feeds
Orchard.Tokens.HtmlFilter
Orchard.Users
Orchard.Users.Workflows
Orchard.Users.PasswordEditor
Orchard.Warmup
Orchard.Widgets
Orchard.Widgets.PageLayerHinting
Orchard.Widgets.ControlWrapper
Orchard.Widgets.Elements
Orchard.Workflows
Orchard.Workflows.Timer
SysCache
TinyMce
Upgrade
Common
Containers
Contents
Contents.ControlWrapper
Dashboard
Feeds
Navigation
Reports
Scheduling
Settings
Shapes
Title
XmlRpc
SafeMode
TheAdmin
TheBootstrapMachine
TheThemeMachine
 */ 