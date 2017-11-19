using MahApps.Metro.Controls.Dialogs;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.ViewModels;
using DataCollector.Client.UI.ViewModels.Core;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DataCollector.Client.UI.Extensions;
using DataCollector.Client.UI.Views.Core;
using LiveCharts.Configurations;
using LiveCharts;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.Users;

namespace DataCollector.Client.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            //unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var application = new App();

            application.InitializeComponent();
            application.Run();
        }

        #region Overrides
        protected override void OnStartup(StartupEventArgs e)
        {
            //register the services
            ServiceLocator.Register();
            //charts
            InitializeChartsLayout();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //send a logout signal request
            var loggedUser = ViewModelBase.MainViewModel?.CurrentLoggedUser;
            if (loggedUser != null)
            {
                IUsersManagementService managementService = ServiceLocator.Resolve<IUsersManagementService>();
                managementService.RecordLogoutTimeStamp(loggedUser.SessionId);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa unhandled exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Exception ex = e.ExceptionObject as Exception;
                MessageBox.Show("Wystąpił wyjątek podczas wykonywania programu. \r\n" + ex.Message, "Kolektor danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// The chartslayout initialization.
        /// </summary>
        private void InitializeChartsLayout()
        {
            //for the measures with id
            var columnMapper = Mappers.Xy<SeriesModel>()
                .X((obj, index) => index)
                .Y(city => city.Value);
            Charting.For<SeriesModel>(columnMapper);
        }
        #endregion
    }
}
