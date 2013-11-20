using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Services.Interfaces
{
    public interface ITagService
    {
        IEnumerable<Tag> GetTagList();

        Tag GetTag(System.Guid id);

        Tag GetTag(string name);

        void CreateTag(Tag tag);

        void UpdateTag(Tag tag);

        void DeleteTag(System.Guid id);
    }
}
