using MahApps.Metro.Controls;
using DataCollector.Client.UI.ViewModels.Core;
using System.Diagnostics;
using System.Windows;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.DataAccess.Interfaces;
using System;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.Communication.Interfaces;
using System.Threading;
using System.Windows.Markup;
using System.Globalization;
using MaterialDesignThemes.Wpf;
using DataCollector.Client.UI.ViewModels.Dialogs;
using DataCollector.Client.UI.ViewModels;

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

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            //dialog postępu
            var progress = await DialogManager.ShowProgressAsync(this, Title, "Inicjalizacja aplikacji");
            progress.SetIndeterminate();

            //referencja do serwisu zarzadzającego dostępeme do bazy danych
            bool success = await CheckDbAccessAsync();

            //raport
            progress.SetMessage(success ? "Wykryto połączenie z bazą danych" : "Wystąpił błąd podczas próby dostępu do bazy danych");
            await Task.Delay(1000);

            if (!success)
            {
                await dialogAccess.ShowMessage("Brak dostępu do bazy danych, sprawdź połączenie lub spróbuj zainstalować aplikację ponownie.");
                this.Close();
                Application.Current.Shutdown();
            }

            progress.SetMessage("Zakończono inicjalizację aplikacji");
            await Task.Delay(1000);
            //zamknięcie dialogu
            await progress.CloseAsync();

            ServiceLocator.Resolve<ICommunication>().Start();
        }
        /// <summary>
        /// Metoda sprawdzająca dostęp do bazy danych.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckDbAccessAsync()
        {
            var settings = ServiceLocator.Resolve<IAppSettings>();
#if DEBUG
            settings.DatabaseConnectionString = "Data source=.\\SQLEXPRESS;Initial catalog=DataCollectorDb;USER ID=sa;PASSWORD=anakonda231;";
#endif
            //w tej wersji aplikacji ustaw ten sam conn string dla bazy użytkowników
            //i pomiarów
            return await Task.Run(() =>
            {
                var usersManagement = ServiceLocator.Resolve<IUsersManagement>();
                var measureAccess = ServiceLocator.Resolve<IMeasureAccess>();
                var measureCollector = ServiceLocator.Resolve<IMeasureCollector>();
                bool success = usersManagement.TryApplyConnectionString(settings.DatabaseConnectionString) &&
                                measureAccess.TryApplyConnectionString(settings.DatabaseConnectionString) &&
                                measureCollector.TryApplyConnectionString(settings.DatabaseConnectionString);
                return success;
            });
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
