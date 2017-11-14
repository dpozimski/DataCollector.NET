using MahApps.Metro.Controls;
using DataCollector.Client.UI.ViewModels.Core;
using System.Windows;
using DataCollector.Client.UI.ModulesAccess;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using System.Threading;
using System.Windows.Markup;
using System.Globalization;
using DataCollector.Client.UI.ViewModels;
using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.Users;
using DataCollector.Client.UI.Extensions;
using WPFLocalizeExtension.Providers;
using WPFLocalizeExtension.Engine;
using DataCollector.Client.Translation;

namespace DataCollector.Client.UI.Views.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        #region ctor
        /// <summary>
        /// Konstruktor klasy Shell.
        /// </summary>
        public Shell()
        {
            //ustawienie wskaźnika na referencję view modelu
            var vm = new ShellViewModel();
            ViewModelBase.MainViewModel = vm;
            this.DataContext = vm;
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private async void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            IDialogAccess dialogAccess = ServiceLocator.Resolve<IDialogAccess>();
            dialogAccess.SetHwnd(this);
            //dialog postępu
            var progress = await DialogManager.ShowProgressAsync(this, Title, "ClientInitialization");
            progress.SetIndeterminate();
            progress.SetMessage(TranslationExtension.GetString("ClientInitializationFinished"));
            await Task.Delay(1000);
            progress.SetMessage(TranslationExtension.GetString("ConnectingWithService"));
            await ConnectToService();
            //zamknięcie dialogu
            await progress.CloseAsync(); 
        }
        private async Task ConnectToService()
        {
            ICommunicationService service = ServiceLocator.Resolve<ICommunicationService>();
            await service.RegisterCallbackChannelAsync();
            if (!service.IsStarted())
            {
                await service.AddSimulatorDeviceAsync();
                service.Start();
            }
        }
        /// <summary>
        /// Zdarzenie powodujące przerzucenie focus na okno główne programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            this.dialogHost.Focus();
        }
        #endregion
    }
}
