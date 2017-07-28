using CodeSanook.Configuration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace Orchard.Users.Handlers {
    public class CodeSanookModuleSettingPartHandler  : ContentHandler {

        public Localizer T { get; set; }


            
        public CodeSanookModuleSettingPartHandler (IRepository<CodeSanookModuleSettingPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<CodeSanookModuleSettingPart>("Site"));

            //Filters.Add(new TemplateFilterForPart<CodeSanookModuleSettingPart>(
            //    "CodeSanook", "Parts/CodeSanookModuleSetting", "CodeSanook"));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("CodeSanook")));
        }
    }
}