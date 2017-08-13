using Orchard.ContentManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CodeSanook.Comment.Models
{
    public class CommentPart : ContentPart<CommentPartRecord>
    {
        [Required, DisplayName("Comment")]
        public string CommentBody
        {
            get { return Record.CommentBody; }
            set { Record.CommentBody = value; }
        }

        public DateTime? CreatedUtcDate
        {
            get { return Record.CreatedUtcDate; }
            set { Record.CreatedUtcDate = value; }
        }

        public DateTime? LastUpdatedUtcDate
        {
            get { return Record.LastUpdatedUtcDate; }
            set { Record.LastUpdatedUtcDate = value; }
        }
    }
}