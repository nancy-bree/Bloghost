using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        IEnumerable<Tag> GetBlogTags();

        Tag GetTagByName(string name);
    }
}
