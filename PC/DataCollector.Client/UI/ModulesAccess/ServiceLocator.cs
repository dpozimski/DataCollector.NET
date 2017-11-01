using Autofac;
using Autofac.Integration.Wcf;
using DataCollector.Client.UI.Collector;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Klasa implementująca wzorzec ServiceLocator.
    /// </summary>
    public static class ServiceLocator
    {
        #region Private Static Fields
        private static bool isAlreadyRegistered;
        private static IContainer container;
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Rejestracja dostępu do obiektów.
        /// </summary>
        public static void Register()
        {
            if (isAlreadyRegistered)
                throw new InvalidOperationException("Serwisy zostały już zainicjalizowane.");

            var builder = new ContainerBuilder();

            builder.RegisterType<CommunicationServiceCallback>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c =>
            {
                var settingsService = c.Resolve<IAppSettings>();
                return new DuplexChannelFactory<ICommunicationService>(
                    c.Resolve<ICommunicationServiceCallback>(),
                    new WSDualHttpBinding(),
                    new EndpointAddress(settingsService.DeviceCommunicationHost));
            }).SingleInstance();

            builder.Register(c => c.Resolve<DuplexChannelFactory<ICommunicationService>>().CreateChannel())
                .As<ICommunicationService>()
                .UseWcfSafeRelease();

            CreateWcfBasicServiceReference<IUsersManagementService>(builder, s => s.UsersHost);
            CreateWcfBasicServiceReference<IMeasureCollectorService>(builder, s => s.CollectorServiceHost);
            CreateWcfBasicServiceReference<IMeasureAccessService>(builder, s => s.DataAccessHost);


            builder.RegisterType<AppSettings>().As<IAppSettings>().SingleInstance();
            builder.RegisterType<DialogAccess>().As<IDialogAccess>().SingleInstance();

            container = builder.Build();

            isAlreadyRegistered = true;
        }

        /// <summary>
        /// Zwraca obiekt ze wskazanym interfejsem.
        /// </summary>
        /// <typeparam name="TAccessObj"></typeparam>
        /// <returns></returns>
        public static TAccessObj Resolve<TAccessObj>()
        {
            return container.Resolve<TAccessObj>();
        }
        #endregion

        private static void CreateWcfBasicServiceReference<TService>(ContainerBuilder builder, Func<IAppSettings, string> hostFactory)
        {
            builder.Register(c =>
            {
                IAppSettings settings = c.Resolve<IAppSettings>();
                return new ChannelFactory<TService>(
                                new BasicHttpBinding(),
                                new EndpointAddress(hostFactory(settings)));                
            }).SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<TService>>().CreateChannel())
              .As<TService>()
              .UseWcfSafeRelease();
        }
    }
}
