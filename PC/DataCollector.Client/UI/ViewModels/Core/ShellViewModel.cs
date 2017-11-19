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
    /// The shell view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.RootViewModelBase" />
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
        /// Gets or sets the current logged user.
        /// </summary>
        /// <value>
        /// The current logged user.
        /// </value>
        public UserViewModel CurrentLoggedUser
        {
            get { return currentLoggedUser; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentLoggedUser, value);

                //sets the trigger to logout
                if (currentLoggedUser != null)
                    currentLoggedUser.ObservableForProperty(vm => vm.LogoutRequested, s => s).
                        Subscribe(c => LogoutMethod(c));
            }
        }
        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        /// <value>
        /// The selected device.
        /// </value>
        public MeasureDeviceViewModel SelectedDevice
        {
            get { return selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref selectedDevice, value); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is user information flyout open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is user information flyout open; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserInfoFlyoutOpen
        {
            get { return isUserInfoFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref isUserInfoFlyoutOpen, value); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is devices flyout open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is devices flyout open; otherwise, <c>false</c>.
        /// </value>
        public bool IsDevicesFlyoutOpen
        {
            get { return isDevicesFlyoutOpen; }
            set { this.RaiseAndSetIfChanged(ref isDevicesFlyoutOpen, value); }
        }
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        public string Now
        {
            get { return DateTime.Now.ToString(CultureInfo.GetCultureInfo("pl-PL")); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the led change dialog command.
        /// </summary>
        /// <value>
        /// The led change dialog command.
        /// </value>
        public ReactiveCommand<object> LedChangeDialogCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the login to application command.
        /// </summary>
        /// <value>
        /// The login to application command.
        /// </value>
        public ReactiveCommand<object> LoginToApplicationCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the users management dialog command.
        /// </summary>
        /// <value>
        /// The users management dialog command.
        /// </value>
        public ReactiveCommand<object> UsersManagementDialogCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the settings prompt dialog command.
        /// </summary>
        /// <value>
        /// The settings prompt dialog command.
        /// </value>
        public ReactiveCommand<object> SettingsPromptDialogCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the toggle devices flyout command.
        /// </summary>
        /// <value>
        /// The toggle devices flyout command.
        /// </value>
        public ReactiveCommand<object> ToggleDevicesFlyoutCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the flyout user command.
        /// </summary>
        /// <value>
        /// The flyout user command.
        /// </value>
        public ReactiveCommand<object> FlyoutUserCommand { get; protected set; }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
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
        /// Settingses the prompt dialog method.
        /// </summary>
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
        /// Logins to application method asynchronous.
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
        /// Logouts the method.
        /// </summary>
        /// <param name="logoutTrigger">if set to <c>true</c> [logout trigger].</param>
        private async void LogoutMethod(bool logoutTrigger)
        {
            string text = TranslationExtension.GetString("DoYouReallyWantToLogoutFromTheCurrentSession");
            var builder = RequestModel.Create().Text(text).Style(MessageDialogStyle.AffirmativeAndNegative);
            var message = await DialogAccess.ShowRequestAsync(builder);
            if (message == MessageDialogResult.Affirmative)
            {
                //send a request with logout timestamp
                usersManagement.RecordLogoutTimeStamp(CurrentLoggedUser.SessionId);
                //clears the current logged user
                CurrentLoggedUser = null;
                //close the flyout
                IsUserInfoFlyoutOpen = false;
            }
        }
        /// <summary>
        /// Initializes the timer.
        /// </summary>
        private void InitTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (o, e) => this.RaisePropertyChanged(nameof(Now));
            timer.Start();
        }
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            this.SettingsPromptDialogCommand = ReactiveCommand.Create(GetUserCondition(UserRole.Administrator));
            this.SettingsPromptDialogCommand.Subscribe(async s => await SettingsPromptDialogMethod());
            this.ToggleDevicesFlyoutCommand = ReactiveCommand.Create(GetUserCondition(UserRole.All));
            this.ToggleDevicesFlyoutCommand.Subscribe(s => IsDevicesFlyoutOpen = !IsDevicesFlyoutOpen);
            this.FlyoutUserCommand = ReactiveCommand.Create();
            this.FlyoutUserCommand.Subscribe(async s => await FlyoutUserMethod());
            this.UsersManagementDialogCommand = ReactiveCommand.Create(GetUserCondition(UserRole.Administrator));
            this.UsersManagementDialogCommand.Subscribe(async s => await ShowUsersManagementDialog());
            this.LoginToApplicationCommand = ReactiveCommand.Create();
            this.LoginToApplicationCommand.Subscribe(async s => await LoginToApplicationMethodAsync());
            this.LedChangeDialogCommand = ReactiveCommand.Create(Observable.CombineLatest(GetUserCondition(UserRole.All),
                                                                                          this.ObservableForProperty(s=>s.SelectedDevice),
                                                                                          (a,b) => a && b.Value != null));
            this.LedChangeDialogCommand.Subscribe(async s => await ShowLedStateManager());
        }
        /// <summary>
        /// Shows the led state manager.
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
        /// Flyouts the user method.
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
        /// Gets the user condition.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        private IObservable<bool> GetUserCondition(UserRole role)
        {
            return this.ObservableForProperty(s => s.CurrentLoggedUser,
                                                user => user != null && role.HasFlag(user.Role));
        }
        /// <summary>
        /// Shows the users management dialog.
        /// </summary>
        /// <returns></returns>
        private async Task ShowUsersManagementDialog()
        {
            var view = new UsersManagementViewDialog();
            view.DataContext = new UsersManagementDialogViewModel();

            await DialogAccess.Show(view, RootDialogId);
        }
        #endregion
    }
}
