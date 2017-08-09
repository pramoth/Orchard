using System;

namespace CodeSanook.Comment.Models {
    public enum CommentStatus {
        Pending,
        Approved,

        [Obsolete]
        Spam
    }
}