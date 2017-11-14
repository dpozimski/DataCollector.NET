using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using DataCollector.Client.UI.Views.Core;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.Views.Dialogs;
using DataCollector.Client.UI.ViewModels.Dialogs;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.Users;
using netoaster.Enumes;
using DataCollector.Client.Translation;

namespace DataCollector.Client.UI.ViewModels.Core
{
    /// <summary>
    /// Klasa ViewModel implementująca obsługę okna głównego aplikacji.
    /// </summary>
    public class ShellViewModel : RootViewModelBase
    {
        #region Private Fields
        private IUsersManagementService usersManagement;
        private bool isDevicesFlyoutOpen, isUserInfoFlyoutOpen;
        private IAppSettings settings;
        private UserViewModel currentLoggedUser;
        private MeasureDeviceViewModel selectedDevice;
        #endregion

        #region Public Properties
        /// <summary>
        /// Aktualnie zalogowany użytkownik.
        /// </summary>
        public UserViewModel CurrentLoggedUser
        {
            get { return currentLoggedUser; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentLoggedUser, value);

                //ustawienie triggera na komendę wylogowania się
                if (currentLoggedUser != null)
                    currentLoggedUser.ObservableForProperty(vm => vm.LogoutRequested, s => s).
                        Subscribe(c => LogoutMethod(c));
            }
        }
        /// <summary>
        /// Wybrane urządzenie pomiarowe
        /// </summary>
        public MeasureDeviceViewModel SelectedDevice
        {
            get { return selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref selectedDevice, value); }
        }
        /// <summary>
        /// Sterowanie panelem informacji o użytkowniku.
        /// </summary>
        public bool IsUserInfoFlyoutOpen
        {
            get { return isUserInfoFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref isUserInfoFlyoutOpen, value); }
        }
        /// <summary>
        /// Sterowanie panelem urządzeń.
        /// </summary>
        public bool IsDevicesFlyoutOpen
        {
            get { return isDevicesFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref isDevicesFlyoutOpen, value); }
        }
        /// <summary>
        /// Reprezentuje aktualną godzinę w systemie.
        /// </summary>
        public string Now
        {
            get { return DateTime.Now.ToString(CultureInfo.GetCultureInfo("pl-PL")); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda sterowania stanem diody LED.
        /// </summary>
        public ReactiveCommand<object> LedChangeDialogCommand { get; protected set; }
        /// <summary>
        /// Komenda logowania do aplikacji.
        /// </summary>
        public ReactiveCommand<object> LoginToApplicationCommand { get; protected set; }
        /// <summary>
        /// Komenda wyzwalana podczas żądania wyświetlenia zarządzania użytkownikami.
        /// </summary>
        public ReactiveCommand<object> UsersManagementDialogCommand { get; protected set; }
        /// <summary>
        /// Komenda wyzwalana podczas żądania wyświetlenia ustawień.
        /// </summary>
        public ReactiveCommand<object> SettingsPromptDialogCommand { get; protected set; }
        /// <summary>
        /// Komenda wyzwalana podczas żądania zmiany wyświetlenia listy urządzeń.
        /// </summary>
        public ReactiveCommand<object> ToggleDevicesFlyoutCommand { get; protected set; }
        /// <summary>
        /// Komenda wyzwalana podczas żądania wyświetlenia informacji o użytkowniku.
        /// użytkowniku.
        /// </summary>
        public ReactiveCommand<object> FlyoutUserCommand { get; protected set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy ShellViewModel.
        /// </summary>
        public ShellViewModel()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
#endif
            usersManagement = ServiceLocator.Resolve<IUsersManagementService>();
            settings = Locator.Current.GetService<IAppSettings>();
            InitTimer();
            InitCommands();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Wyswietla ustawienia oraz aktualizuje nowe ustawienia.
        /// </summary>
        /// <param name="shell">referencja do okna głównego</param>
        /// <returns></returns>
        public async Task SettingsPromptDialogMethod()
        {
            var settings = new SettingsDialogView();
            settings.DataContext = new SettingsDialogViewModel();
            IAppSettings result = await DialogAccess.Show(settings, RootDialogId) as IAppSettings;
            if (result != null)
            {
                var appSettings = ServiceLocator.Resolve<IAppSettings>();
                appSettings.RunAppDuringStartup = result.RunAppDuringStartup;
            }
        }
        /// <summary>
        /// Metoda logująca użytkownika do aplikacji.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoginToApplicationMethodAsync()
        {
            bool success = false;

            var result = await DialogAccess.ShowLoginAsync();
            if (result != null)
            {
                success = usersManagement.ValidateCredentials(result.Username, result.Password);
                if (!success)
                    DialogAccess.ShowToastNotification(TranslationExtension.GetString("InvalidLoginData"));
                else
                    CurrentLoggedUser = new UserViewModel(usersManagement.GetUser(result.Username));
            }
            return success;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa wylogowania się z aplikacji.
        /// </summary>
        /// <param name="logoutTrigger"></param>
        private async void LogoutMethod(bool logoutTrigger)
        {
            string text = TranslationExtension.GetString("DoYouReallyWantToLogoutFromTheCurrentSession");
            var builder = RequestModel.Create().Text(text).Style(MessageDialogStyle.AffirmativeAndNegative);
            var message = await DialogAccess.ShowRequestAsync(builder);
            if (message == MessageDialogResult.Affirmative)
            {
                //odznaka wylogowania się użytkownika z systemu
                usersManagement.RecordLogoutTimeStamp(CurrentLoggedUser.SessionId);
                //wyczysć aktualnie zalogowanego użytkownika
                CurrentLoggedUser = null;
                //zamknij flyout użytkownika
                IsUserInfoFlyoutOpen = false;
            }
        }
        /// <summary>
        /// Inicjalizacja aktualizacji czasu.
        /// </summary>
        private void InitTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (o, e) => this.RaisePropertyChanged(nameof(Now));
            timer.Start();
        }
        /// <summary>
        /// Inicjalizacji komend.
        /// </summary>
        private void InitCommands()
        {
            //inicjalizacja komendy ustawień
            this.SettingsPromptDialogCommand = ReactiveCommand.Create(GetUserCondition(UserRole.Administrator));
            this.SettingsPromptDialogCommand.Subscribe(async s => await SettingsPromptDialogMethod());
            //zezwolenie na wykonanie komendy jeżeli liczba urządzeń jest większa od zero
            //lub flyout jest otwarty
            this.ToggleDevicesFlyoutCommand = ReactiveCommand.Create(GetUserCondition(UserRole.All));
            this.ToggleDevicesFlyoutCommand.Subscribe(s => IsDevicesFlyoutOpen = !IsDevicesFlyoutOpen);
            //informacje o użytkowniku - jeśli istnieje
            //lub żądanie logowania
            this.FlyoutUserCommand = ReactiveCommand.Create();
            this.FlyoutUserCommand.Subscribe(async s => await FlyoutUserMethod());
            //zarządzanie użytkownikami
            this.UsersManagementDialogCommand = ReactiveCommand.Create(GetUserCondition(UserRole.Administrator));
            this.UsersManagementDialogCommand.Subscribe(async s => await ShowUsersManagementDialog());
            //inicjalizacja komendy logowania do aplikacji
            this.LoginToApplicationCommand = ReactiveCommand.Create();
            this.LoginToApplicationCommand.Subscribe(async s => await LoginToApplicationMethodAsync());
            //incijalizacja komendy sterowania diodą LED
            this.LedChangeDialogCommand = ReactiveCommand.Create(Observable.CombineLatest(GetUserCondition(UserRole.All),
                                                                                          this.ObservableForProperty(s=>s.SelectedDevice),
                                                                                          (a,b) => a && b.Value != null));
            this.LedChangeDialogCommand.Subscribe(async s => await ShowLedStateManager());
        }
        /// <summary>
        /// Wyswietla menadżera stanu diody LED.
        /// </summary>
        /// <returns></returns>
        private async Task ShowLedStateManager()
        {
            if (selectedDevice.IsConnected)
            {
                var view = new LedManagerViewDialog();
                view.DataContext = new LedManagerViewModel();
                await DialogAccess.Show(view, RootDialogId);
            }
            else
                DialogAccess.ShowToastNotification(TranslationExtension.GetString("ThereIsNoConnectionWithDevice"), ToastType.Error);
        }
        /// <summary>
        /// Metoda zmieniająca stan okna info o użytkowniku lub logująca do systemu
        /// </summary>
        /// <returns></returns>
        private async Task FlyoutUserMethod()
        {
            if (CurrentLoggedUser != null)
                IsUserInfoFlyoutOpen = !IsUserInfoFlyoutOpen;
            else
                await LoginToApplicationMethodAsync();
        }
        /// <summary>
        /// Zwraca obserwatora nasłuchującego na spełnienie określonego warunku podczas zmiany stanu użytkownika.
        /// dla określonego uprawnienia.
        /// </summary>
        /// <param name="role">wymagane uprawnienie</param>
        /// <returns></returns>
        private IObservable<bool> GetUserCondition(UserRole role)
        {
            return this.ObservableForProperty(s => s.CurrentLoggedUser,
                                                user => user != null && role.HasFlag(user.Role));
        }
        /// <summary>
        /// Wyświetla dialog zarządzania użytkownikami.
        /// </summary>
        private async Task ShowUsersManagementDialog()
        {
            var view = new UsersManagementViewDialog();
            view.DataContext = new UsersManagementDialogViewModel();

            await DialogAccess.Show(view, RootDialogId);
        }
        #endregion
    }
}
