using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class CommentContext
    {
        public CommentContext Comment { get; set; }
        private IChooseCommentStrategy _ChooseCommentStrategy;

        public CommentContext(IChooseCommentStrategy Comment)
        {
            this._ChooseCommentStrategy = Comment;
        }
        public void SetStrategy(IChooseCommentStrategy Comment)
        {
            this._ChooseCommentStrategy = Comment;
        }

        public string GetComment()
        {
           return _ChooseCommentStrategy.Comment();
        }

        public void ChooseComment()
        {
            Console.WriteLine("please text city");
        }

    }
}
