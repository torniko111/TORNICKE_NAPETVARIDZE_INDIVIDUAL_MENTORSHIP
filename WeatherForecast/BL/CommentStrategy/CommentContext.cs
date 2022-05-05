using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class CommentContext
    {
        public CommentContext comment { get; set; }
        private IChooseCommentStrategy ChooseCommentStrategy;

        public CommentContext(IChooseCommentStrategy Comment)
        {
            this.ChooseCommentStrategy = Comment;
        }
        public void SetStrategy(IChooseCommentStrategy Comment)
        {
            this.ChooseCommentStrategy = Comment;
        }

        public string Comment()
        {
           return ChooseCommentStrategy.Comment();
        }

        public void ChooseComment()
        {
            Console.WriteLine("please text city");
        }

    }
}
