using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Bloghost.Core.Repositories;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate
{
    public abstract class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected ISession Session { get { return NHibernateUnitOfWork.Current.Session; } }

        public void Create(TEntity entity)
        {
            Session.Save(entity);
        }

        public TEntity Get(Guid id)
        {
            return Session.Get<TEntity>(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Session.Query<TEntity>().ToList();
        }

        public void Update(TEntity entity)
        {
            if (entity.GetType().GetProperty("Modified") != null)
            {
                entity.GetType().GetProperty("Modified").SetValue(entity, DateTime.Now);
            }
            Session.SaveOrUpdate(entity);
        }

        public void Delete(Guid id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }
    }
}
