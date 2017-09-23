using CodeSanook.Comment.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.Comment.Handlers
{
    public class CommentPartHandler:ContentHandler
    {
        public CommentPartHandler(IRepository<CommentPartRecord> repository)
        {
            //register repository
            Filters.Add(StorageFilter.For(repository));
        }
    }
}