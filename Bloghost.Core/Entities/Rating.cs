using System;

namespace Bloghost.Core.Entities
{
    public class Rating : BaseEntity
    {
        public virtual Guid CommentID { get; set; }

        public virtual Guid UserID { get; set; }

        public virtual int Vote { get; set; }
    }
}
