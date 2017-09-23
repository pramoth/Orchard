using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Models
{
    public class CommentItemViewModel
    {
        public ContentItem Comment { get; set; } 
        public ContentItem User { get; set; } 
    }
}