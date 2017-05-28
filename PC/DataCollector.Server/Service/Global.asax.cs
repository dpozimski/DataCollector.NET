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
                cfg.CreateMap<IDeviceInfo, DeviceHandlers.Models.DeviceInfo>();
            });

            //Autofac
            var builder = new ContainerBuilder();

            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(s => !s.Name.EndsWith("DeviceHandler"))
                   .AsImplementedInterfaces()
                   .Except<RestDeviceHandlerConfiguration>()
                   .Except<WebCommunicationService>(ct => ct.WithParameter("port", GetSettingsValue("DeviceCommunicationPort")).SingleInstance())
                   .Except<CachedDetectedDevicesContainer>(ct => ct.As<IDetectedDevicesContainer>().WithParameter("cleanupCacheInterval", TimeSpan.FromSeconds(GetSettingsValue("CleanupCacheInterval"))))
                   .Except<MeasureAccessService>(ConstructWithConnectionString)
                   .Except<MeasureCollectorService>(ConstructWithConnectionString)
                   .Except<UsersManagementService>(ConstructWithConnectionString);
            builder.Register(s => new RestDeviceHandlerConfiguration()
            {
                GetMeasurementsRequest = ConfigurationManager.AppSettings["GetMeasurementsRequest"],
                LedChangeRequest = ConfigurationManager.AppSettings["LedChangeRequest"],
                LedStateRequest = ConfigurationManager.AppSettings["LedStateRequest"],
            }).As<IDeviceHandlerConfiguration>();

            AutofacHostFactory.Container = builder.Build();
        }

        /// <summary>
        /// Podanie argumentu danych połączeniowych do buildera.
        /// </summary>
        /// <param name="ct"></param>
        private void ConstructWithConnectionString<T>(IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> ct)
            => ct.WithParameter("ConnectionString", GetSettingsValue("ConnectionString"));

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