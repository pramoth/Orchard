using System;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CodeSanook.Comment.Models
{
    public class CommentPartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public virtual string CommentBody { get; set; }
        public virtual DateTime? CreatedUtcDate { get; set; }
        public virtual DateTime? LastUpdatedUtcDate { get; set; }
        public virtual CommentContainerPartRecord CommentContainerPartRecord { get; set; }
    }
}
