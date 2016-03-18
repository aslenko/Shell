using Autofac;
using Autofac.Integration.WebApi;
using Shell.Model;
using System.Reflection;
using System.Web.Http;

namespace Shell.API
{
    /// <summary>
    /// Configuration class.
    /// </summary>
    public static class AutofacConfig
    {
        #region Public Methods
        /// <summary>
        /// Initialize a new AutoFac container as a dependency resolver in the HTTP configuration.
        /// </summary>
        /// <param name="config"></param>
        public static void Configure(HttpConfiguration config)
        {
            //
            // Called by asp.net, during application startup.
            // Configures the auto-fac container with real types.
            //

            ContainerBuilder containerBuilder = new ContainerBuilder();
            IContainer container = RegisterServices(containerBuilder);
            Configure(config, container);
        }

        /// <summary>
        /// Initialize the specified AutoFac container as a dependency resolver in the HTTP configuration.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public static void Configure(HttpConfiguration config, IContainer container)
        {
            //
            // Note that the container may have real types, or moqs.
            // Moqs would be configured during unit tests (for example).
            //

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);       
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Return a new container, with all application dependencies registered.
        /// </summary>
        /// <param name="builder">The container builder</param>
        /// <returns>IContainer</returns>
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EntityContext>().As<EntityContext>().InstancePerRequest();
            //builder.RegisterType<SynchronizationContext>().As<ISynchronizationContext>().InstancePerRequest();
            return builder.Build();
        }
        #endregion
    }
}
