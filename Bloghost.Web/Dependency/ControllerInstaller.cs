using System.Web.Mvc;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace Bloghost.Web.Dependency
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                //All MVC controllers
            Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),
            Component.For<Infrastructure.BloghostMembershipProvider>().LifeStyle.Transient.Named("BloghostProvider")
            );
        }
    }
}