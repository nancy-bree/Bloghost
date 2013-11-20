using System;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateCommentRepository : NHibernateRepository<Comment>, ICommentRepository
    {
    }
}
