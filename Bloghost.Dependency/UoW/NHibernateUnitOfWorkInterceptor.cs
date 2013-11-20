using System;
using System.Reflection;
using NHibernate;
using Bloghost.Data.NHibernate;

namespace Bloghost.Dependency.UoW
{
    public class NHibernateUnitOfWorkInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateUnitOfWorkInterceptor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            if (NHibernateUnitOfWork.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            try
            {
                NHibernateUnitOfWork.Current = new NHibernateUnitOfWork(_sessionFactory);
                NHibernateUnitOfWork.Current.BeginTransaction();

                try
                {
                    invocation.Proceed();
                    NHibernateUnitOfWork.Current.Commit();
                }
                catch
                {
                    try
                    {
                        NHibernateUnitOfWork.Current.Rollback();
                    }
                    catch
                    {

                    }

                    throw;
                }
            }
            finally
            {
                NHibernateUnitOfWork.Current = null;
            }
        }

        private static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            if (UnitOfWorkHelper.HasUnitOfWorkAttribute(methodInfo))
            {
                return true;
            }

            if (UnitOfWorkHelper.IsRepositoryMethod(methodInfo))
            {
                return true;
            }

            return false;
        }
    }
}
