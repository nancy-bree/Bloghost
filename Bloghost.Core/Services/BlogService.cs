using System.Collections.Generic;
using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using Bloghost.Core.Services.Interfaces;

namespace Bloghost.Core.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [UnitOfWork]
        public IEnumerable<Blog> GetBlogList()
        {
            return _blogRepository.GetAll().OrderBy(x => x.User.CreatedDate).ToList();
        }

        [UnitOfWork]
        public void CreateBlog(Blog blog)
        {
            _blogRepository.Create(blog);
        }

        [UnitOfWork]
        public void UpdateBlog(Blog blog)
        {
            _blogRepository.Update(blog);
        }

        [UnitOfWork]
        public void DeleteBlog(System.Guid id)
        {
            _blogRepository.Delete(id);
        }

        [UnitOfWork]
        public Blog GetBlog(System.Guid id)
        {
            return _blogRepository.Get(id);
        }

        [UnitOfWork]
        public System.Guid GetBlogID(string login)
        {
            return _blogRepository.GetBlogIdByUserName(login);
        }

        [UnitOfWork]
        public System.Guid GetBlogID(System.Guid userId)
        {
            return _blogRepository.GetBlogIdByUserId(userId);
        }
    }
}
