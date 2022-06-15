using BL.CommentStrategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class WarmlyStrategy : IChooseCommentStrategy
    {
        public string Comment()
        {
            return CommentConstants.WarmlyComment;
        }
    }
}
