using Windows.ApplicationModel.Background;
using System;
using System.Diagnostics;
using DataCollector.Device.BusDevice;
using DataCollector.Device.Models;
using System.Linq;
using DataCollector.Device.Data;
using DataCollector.Device.Controller;
using Autofac;
using System.Reflection;
using DataCollector.Device.BusDevice.Module;

namespace DataCollector.Device.Task
{
    /// <summary>
    /// Entry point for the task.
    /// </summary>
    public sealed class StartupTask : IBackgroundTask
    {
        #region Fields
        private ILifetimeScope lifeTimeScope;
        private IContainer container;
        private BackgroundTaskDeferral deferral;
        #endregion

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //gets the allocated resources for this task
            deferral = taskInstance.GetDeferral();
            //register a container
            var assembly = Assembly.Load(new AssemblyName("DataCollector.Device"));
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Module"))
                   .As<I2CBusDevice>()
                   .Except<PCF8574Module>(ct => ct.As(typeof(I2CBusDevice), typeof(ILedController)).SingleInstance());
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Controller"))
                   .AsSelf()
                   .Except<WebResponseController>()
                   .Except<JsonMeasuresDataController>(ct => ct.As<IMeasuresDataController>());
            container = builder.Build();

            lifeTimeScope = container.BeginLifetimeScope();

            var busDevicesController = lifeTimeScope.Resolve<BusDevicesController>();
            var networkController = lifeTimeScope.Resolve<NetworkAccessController>();

            busDevicesController.Init();
            networkController.Start();
        }
    }
}
