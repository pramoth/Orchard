using CodeSanook.Configuration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Pluralsight.Movies.Drivers {
    public class CodeSanookModuleSettingPartDriver : ContentPartDriver<CodeSanookModuleSettingPart> {
         protected override string Prefix {
            get { return "CodeSanookModuleSetting"; }
        }

        protected override DriverResult Editor(CodeSanookModuleSettingPart part, 
            dynamic shapeHelper) {
            return ContentShape("Parts_CodeSanook_ModuleSetting",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/CodeSanookModuleSetting",
                    Model: part,
                    Prefix: Prefix))
                    .OnGroup("CodeSanook");
        }

        protected override DriverResult Editor(CodeSanookModuleSettingPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}