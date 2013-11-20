using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<Blog> GetBlogList();

        Blog GetBlog(System.Guid id);

        System.Guid GetBlogID(string login);

        System.Guid GetBlogID(System.Guid userId);

        void CreateBlog(Blog blog);

        void UpdateBlog(Blog blog);

        void DeleteBlog(System.Guid id);
    }
}
