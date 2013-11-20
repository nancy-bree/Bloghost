using System;
using System.Collections.Generic;
using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using NHibernate.Linq;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateTagRepository : NHibernateRepository<Tag>, ITagRepository
    {
        public IEnumerable<Tag> GetBlogTags()
        {
            /*var query = Session.Query<Tag>()
                .Select(x => x.Name)*/
            throw new NotImplementedException();
        }

        public Tag GetTagByName(string name)
        {
            var query = Session.Query<Tag>().FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
            return query;
        }
    }
}
