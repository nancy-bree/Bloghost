using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Services.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetCommentList();

        Comment GetComment(System.Guid id);

        void CreateComment(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteComment(System.Guid id);
    }
}
