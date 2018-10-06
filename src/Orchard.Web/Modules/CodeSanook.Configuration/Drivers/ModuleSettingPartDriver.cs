using CodeSanook.Configuration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Pluralsight.Movies.Drivers {
    public class CodeSanookModuleSettingPartDriver : ContentPartDriver<ModuleSettingPart> {
         protected override string Prefix {
            get { return "ModuleSetting"; }
        }

        protected override DriverResult Editor(ModuleSettingPart part, 
            dynamic shapeHelper) {
            return ContentShape("Parts_ModuleSetting",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/ModuleSetting",
                    Model: part,
                    Prefix: Prefix))
                    .OnGroup("CodeSanook Module");
        }

        protected override DriverResult Editor(ModuleSettingPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}