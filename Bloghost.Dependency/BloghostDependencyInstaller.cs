using System.Reflection;
using Castle.Core;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Bloghost.Core.Services;
using Bloghost.Data.NHibernate;
using Bloghost.Dependency.UoW;

namespace Bloghost.Dependency
{
    public class BloghostDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            container.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(CreateNHibernateSessionFactory).LifeStyle.Singleton,
                Component.For<NHibernateUnitOfWorkInterceptor>().LifeStyle.Transient,
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateBlogRepository))).InSameNamespaceAs<NHibernateBlogRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateCommentRepository))).InSameNamespaceAs<NHibernateCommentRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateEntryRepository))).InSameNamespaceAs<NHibernateEntryRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateRatingRepository))).InSameNamespaceAs<NHibernateRatingRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateTagRepository))).InSameNamespaceAs<NHibernateTagRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHibernateUserRepository))).InSameNamespaceAs<NHibernateUserRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogService))).InSameNamespaceAs<BlogService>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(CommentService))).InSameNamespaceAs<CommentService>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(EntryService))).InSameNamespaceAs<EntryService>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(RatingService))).InSameNamespaceAs<RatingService>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(TagService))).InSameNamespaceAs<TagService>().WithService.DefaultInterfaces().LifestyleTransient(),
                Classes.FromAssembly(Assembly.GetAssembly(typeof(UserService))).InSameNamespaceAs<UserService>().WithService.DefaultInterfaces().LifestyleTransient()
                );
        }

        private static ISessionFactory CreateNHibernateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(c => c.FromConnectionStringWithKey("Bloghost")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Bloghost.Data.NHibernate")))
                //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                //.ExposeConfiguration(cfg => { SchemaExport sche = new SchemaExport(cfg); sche.Drop(true, true); sche.Create(true, true); })
                .BuildSessionFactory();
        }

        void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (UnitOfWorkHelper.IsRepositoryClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NHibernateUnitOfWorkInterceptor)));
            }

            foreach (var method in handler.ComponentModel.Implementation.GetMethods())
            {
                if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NHibernateUnitOfWorkInterceptor)));
                    return;
                }
            }
        }
    }
}
