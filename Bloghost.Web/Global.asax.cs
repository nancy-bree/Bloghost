using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Bloghost.Dependency;
using Bloghost.Web;
using Bloghost.Web.Dependency;
using System.Web.Optimization;

using System.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Bloghost.Data.NHibernate;
using Bloghost.Data.NHibernate.Mapping;

namespace Bloghost.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication, IContainerAccessor
    {
        //private WindsorContainer _windsorContainer;
        private static IWindsorContainer _windsorContainer;
        //private ISessionFactory sf;
        protected void Application_Start()
        {
            InitializeWindsor();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /*sf = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(c => c.FromConnectionStringWithKey("Bloghost")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.Load("Bloghost.Data.NHibernate")))
                //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .ExposeConfiguration(cfg => { SchemaExport sche = new SchemaExport(cfg); sche.Drop(true, true); sche.Create(true, true); })
                .BuildSessionFactory();
            sf.Close();*/
        }

        protected void Application_End()
        {
            if (_windsorContainer != null)
            {
                _windsorContainer.Dispose();
            }
        }

        private void InitializeWindsor()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(FromAssembly.Containing<BloghostDependencyInstaller>());
            _windsorContainer.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_windsorContainer.Kernel));
        }

        public IWindsorContainer Container
        {
            get { return _windsorContainer; }
        }
    }
}