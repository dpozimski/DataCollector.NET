using Autofac;
using Autofac.Integration.Wcf;
using AutoMapper;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
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
                   .Except<WebCommunicationService>(ct =>
                        ct.As<ICommunicationService>().SingleInstance());

            AutofacHostFactory.Container = builder.Build();
        }
    }
}