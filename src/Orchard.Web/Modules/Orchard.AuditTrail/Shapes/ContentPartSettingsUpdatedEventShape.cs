﻿using System.Collections.Generic;
using Orchard.AuditTrail.Helpers;
using Orchard.AuditTrail.Providers.ContentDefinition;
using Orchard.ContentManagement.MetaData.Services;
using Orchard.DisplayManagement.Implementation;

namespace Orchard.AuditTrail.Shapes {
    public class ContentPartSettingsUpdatedEventShape : AuditTrailEventShapeAlteration<ContentPartAuditTrailEventProvider> {
        private readonly ISettingsFormatter _settingsFormatter;
        public ContentPartSettingsUpdatedEventShape(ISettingsFormatter settingsFormatter) {
            _settingsFormatter = settingsFormatter;
        }

        protected override string EventName {
            get { return ContentPartAuditTrailEventProvider.PartSettingsUpdated; }
        }

        protected override void OnAlterShape(ShapeDisplayingContext context) {
            var eventData = (IDictionary<string, object>)context.Shape.EventData;
            var oldSettings = _settingsFormatter.Map(XmlHelper.Parse((string)eventData["OldSettings"]));
            var newSettings = _settingsFormatter.Map(XmlHelper.Parse((string)eventData["NewSettings"]));
            var diff = oldSettings.GetDiff(newSettings);

            context.Shape.OldSettings = oldSettings;
            context.Shape.NewSettings = newSettings;
            context.Shape.Diff = diff;
        }
    }
}