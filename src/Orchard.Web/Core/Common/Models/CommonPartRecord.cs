using System;
using Orchard.ContentManagement.Records;

namespace Orchard.Core.Common.Models {
    public class CommonPartRecord : ContentPartRecord {
        public virtual int OwnerId { get; set; }
        public virtual ContentItemRecord Container { get; set; }
        public virtual DateTime? CwreatedUtc { get; set; }
        public virtual DateTime? PublishedUtc { get; set; }
        public virtual DateTime? ModifiedUtc { get; set; }
    }
}