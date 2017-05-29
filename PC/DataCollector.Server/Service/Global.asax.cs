using Autofac;
using Autofac.Integration.Wcf;
using AutoMapper;
using DataCollector.Server.BroadcastListener;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DeviceHandlers.Adapters;
using DataCollector.Server.DeviceHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Autofac.Builder;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.Interfaces.Communication;
using DataCollector.Server.Interfaces.Data;

namespace DataCollector.Server
{
    /// <summary>
    /// Globalna klasa inicjująca serwis WCF.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Metoda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            //AutoMapper
            Mapper.Initialize(cfg => {
                cfg.CreateMap<IDeviceInfo, MeasureDevice>();
            });

            //Autofac
            var builder = new ContainerBuilder();

            ConstructBroadcastListener(builder);
            ConstructDeviceHandlers(builder);
            ConstructServices(builder);

            AutofacHostFactory.Container = builder.Build();

            //var service = AutofacHostFactory.Container.Resolve<MeasureCollectorService>();
        }

        /// <summary>
        /// Konstrukcja typów do kontenera AutoFac z biblioteki serwisów.
        /// </summary>
        /// <param name="builder">builder</param>
        private void ConstructServices(ContainerBuilder builder)
        {
            var serviceAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(serviceAssembly)
                   .AsImplementedInterfaces()
                   .Except<CommunicationClientCallbacksContainer>(ct => ct.As<ICommunicationClientCallbacksContainer>().SingleInstance())
                   .Except<WebCommunicationService>(ct => ct.WithParameter("port", GetSettingsValue("DeviceCommunicationPort")).As<ICommunicationService>().SingleInstance())
                   .Except<MeasureCollectorService>(ct => ConstructWithConnectionString(ct, _aCt => _aCt.As<IMeasureCollectorService>().SingleInstance()))
                   .Except<MeasureAccessService>(ct => ConstructWithConnectionString(ct, _aCt => _aCt.As<IMeasureAccessService>()))
                   .Except<UsersManagementService>(ct => ConstructWithConnectionString(ct, _aCt => _aCt.As<IUsersManagementService>()));
        }

        /// <summary>
        /// Konstrukcja typów do kontenera AutoFac z biblioteki BroadcastListener.
        /// </summary>
        /// <param name="builder">builder</param>
        private void ConstructBroadcastListener(ContainerBuilder builder)
        {
            var assembly = Assembly.GetAssembly(typeof(IBroadcastScanner));
            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .Except<CachedDetectedDevicesContainer>(ct => ct.As<IDetectedDevicesContainer>().WithParameter("cleanupCacheInterval", TimeSpan.FromSeconds(GetSettingsValue("CleanupCacheInterval"))));
        }

        /// <summary>
        /// Konstrukcja typów do kontenera AutoFac z biblioteki DeviceHandlers.
        /// </summary>
        /// <param name="builder">builder</param>
        private void ConstructDeviceHandlers(ContainerBuilder builder)
        {
            var assembly = Assembly.GetAssembly(typeof(IDeviceHandler));
            builder.RegisterAssemblyTypes(assembly)
                   .Where(s => !s.Name.EndsWith("DeviceHandler"))
                   .AsImplementedInterfaces()
                   .Except<RestDeviceHandlerConfiguration>();
            builder.Register(s => new RestDeviceHandlerConfiguration()
            {
                GetMeasurementsRequest = ConfigurationManager.AppSettings["GetMeasurementsRequest"],
                LedChangeRequest = ConfigurationManager.AppSettings["LedChangeRequest"],
                LedStateRequest = ConfigurationManager.AppSettings["LedStateRequest"],
            }).As<IDeviceHandlerConfiguration>();
        }

        /// <summary>
        /// Podanie argumentu danych połączeniowych do buildera.
        /// </summary>
        /// <param name="ct">builder</param>
        /// <param name="additionalAction">builder akcja dodatkowa</param>
        private void ConstructWithConnectionString<T>(IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> ct, Action<IRegistrationBuilder<T, 
            ConcreteReflectionActivatorData, SingleRegistrationStyle>> additionalAction = null)
        {
            var value = ct.WithParameter("ConnectionString", ConfigurationManager.AppSettings["ConnectionString"]);
            additionalAction?.Invoke(value);
        }

        /// <summary>
        /// Zwraca wartość całkowitą z ustawień aplikacji.
        /// </summary>
        /// <param name="key">klucz</param>
        /// <returns>wartość</returns>
        private int GetSettingsValue(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            return Convert.ToInt32(value);
        }
    }
}