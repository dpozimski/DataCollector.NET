using Autofac;
using Autofac.Integration.Wcf;
using AutoMapper;
using DataCollector.Server.DataFlow.BroadcastListener;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

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
                cfg.CreateMap<IDeviceInfo, DeviceInfo>();
            });

            //Autofac
            var builder = new ContainerBuilder();

            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                   .AsImplementedInterfaces()
                   .Except<WebCommunicationService>().As<ICommunicationService>().WithParameter("port", GetSettingsValue("DeviceCommunicationPort")).SingleInstance()
                   .Except<CachedDetectedDevicesContainer>().As<IDetectedDevicesContainer>().WithParameter("cleanupCacheInterval", TimeSpan.FromSeconds(GetSettingsValue("CleanupCacheInterval")));

            AutofacHostFactory.Container = builder.Build();
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