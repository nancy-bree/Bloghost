using System.Collections.Generic;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using Bloghost.Core.Services.Interfaces;

namespace Bloghost.Core.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [UnitOfWork]
        public IEnumerable<Tag> GetTagList()
        {
            return _tagRepository.GetAll();
        }

        [UnitOfWork]
        public void CreateTag(Tag tag)
        {
            _tagRepository.Create(tag);
        }

        [UnitOfWork]
        public void UpdateTag(Tag tag)
        {
            _tagRepository.Update(tag);
        }

        [UnitOfWork]
        public void DeleteTag(System.Guid id)
        {
            _tagRepository.Delete(id);
        }

        [UnitOfWork]
        public Tag GetTag(System.Guid id)
        {
            return _tagRepository.Get(id);
        }

        [UnitOfWork]
        public Tag GetTag(string name)
        {
            return _tagRepository.GetTagByName(name);
        }
    }
}
