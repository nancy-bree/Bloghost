using System;
using NHibernate;
using Bloghost.Core.Repositories;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        public static NHibernateUnitOfWork Current { get { return _current; } set { _current = value; } }

        [ThreadStatic]
        private static NHibernateUnitOfWork _current;

        public ISession Session { get; private set; }

        private readonly ISessionFactory _sessionFactory;

        private ITransaction _transaction;

        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

        }

        public void BeginTransaction()
        {
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            finally
            {
                Session.Close();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Session.Close();
            }
        }

        public void Dispose()
        {
            if (Session.IsOpen)
            {
                Session.Close();
            }
        }
    }
}
