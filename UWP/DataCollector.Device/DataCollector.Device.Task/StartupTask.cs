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
    /// Klasa stanowiąca punkt wejścia do programu.
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
            //pobranie przydzielonych zasobów na zadanie
            deferral = taskInstance.GetDeferral();
            //rejestracja kontenera i zależności
            var assembly = Assembly.Load(new AssemblyName("DataCollector.Device"));
            var builder = new ContainerBuilder();
            //rejestracja obsługiwanych peryferii w urządzeniu
            //dla pcf8574 przydzielenie dodatkowej roli ILedControllera
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Module"))
                   .As<I2CBusDevice>()
                   .Except<PCF8574Module>(ct => ct.As(typeof(I2CBusDevice), typeof(ILedController)).SingleInstance());
            //rejestracja kontrolerów, za wyjątkiem kontrolera web responses(Restup wymaga własnej fabryki)
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
