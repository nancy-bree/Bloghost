using System;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateRatingRepository : NHibernateRepository<Rating>, IRatingRepository
    {
    }
}
