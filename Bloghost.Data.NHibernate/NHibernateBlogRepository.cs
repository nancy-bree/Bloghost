using System;
using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using NHibernate.Linq;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateBlogRepository : NHibernateRepository<Blog>, IBlogRepository
    {
        public Guid GetBlogIdByUserName(string username)
        {
            var query = Session.Query<Blog>().SingleOrDefault(x => x.User.Login == username);
            return query.ID;
        }

        public Guid GetBlogIdByUserId(Guid id)
        {
            var query = Session.Query<Blog>().SingleOrDefault(x => x.User.ID == id);
            return query.ID;
        }
    }
}
