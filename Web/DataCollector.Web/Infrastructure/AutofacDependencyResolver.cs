using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Users;
using Collector;
using DataAccess;
using Autofac.Core;

namespace DataCollector.Web.Infrastructure
{
    /// <summary>
    /// Provides a configured dependency container.
    /// </summary>
    /// <CreatedOn>28.11.2017 17:10</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    public class AutofacDependencyResolver
    {
        #region [Private Fields]
        private readonly ContainerBuilder m_ContainerBuilder;
        private readonly IConfiguration m_Configuration;
        #endregion

        #region [ctor]        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <CreatedOn>28.11.2017 17:13</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public AutofacDependencyResolver(IConfiguration configuration)
        {
            m_ContainerBuilder = new ContainerBuilder();
            m_Configuration = configuration;
        }
        #endregion

        #region [Public Methods]
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddMvc();
            m_ContainerBuilder.Populate(services);
        }
        public IServiceProvider BuildServiceProvider()
        {
            RegisterAppServices();
            var container = m_ContainerBuilder.Build();
            return container.Resolve<IServiceProvider>();
        }
        #endregion

        #region [Private Methods]
        private void RegisterAppServices()
        {
            //register service clients
            RegisterServiceClient<UsersManagementServiceClient, IUsersManagementService>("UsersHost");
            RegisterServiceClient<MeasureAccessServiceClient, IMeasureAccessService>("DataAccessHost");
            RegisterServiceClient<MeasureCollectorServiceClient, IMeasureCollectorService>("CollectorServiceHost");
        }
        private void RegisterServiceClient<TImplService, TService>(string hostKey)
            where TService : class
            where TImplService : ClientBase<TService>, ICommunicationObject
        {
            m_ContainerBuilder.RegisterType<TImplService>()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("endpointConfiguration", GetEndpointConfigByServiceType<TService>()),
                    new TypedParameter(typeof(string), m_Configuration[hostKey])
                })
                .As<TService>();
        }
        private Enum GetEndpointConfigByServiceType<TService>()
        {
            var ServiceName = typeof(TService).Name;
            if (ServiceName == nameof(IUsersManagementService))
                return UsersManagementServiceClient.EndpointConfiguration.BasicHttpBinding_IUsersManagementService;
            else if (ServiceName == nameof(IMeasureCollectorService))
                return MeasureCollectorServiceClient.EndpointConfiguration.BasicHttpBinding_IMeasureCollectorService;
            else if (ServiceName == nameof(IMeasureAccessService))
                return MeasureAccessServiceClient.EndpointConfiguration.BasicHttpBinding_IMeasureAccessService;
            else
                throw new InvalidOperationException($"The type {ServiceName} is not supported.");
        }
        #endregion
    }
}
