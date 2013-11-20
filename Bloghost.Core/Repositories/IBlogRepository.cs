using System;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Repositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Guid GetBlogIdByUserName(string username);

        Guid GetBlogIdByUserId(Guid id);
    }
}
