using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.Users;
using DataCollector.Client.UI.ViewModels.Core;
using DataCollector.Client.UI.Views.Core;
using DataCollector.Client.UI.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCollector.Client.UI.ViewModels.Dialogs
{
    /// <summary>
    /// ViewModel implementującu zarządzanie użytkownikami.
    /// </summary>
    class UsersManagementDialogViewModel : RootViewModelBase
    {
        #region Private Fields
        private IUsersManagementService usersManagement;
        private ObservableCollection<UserViewModel> users;
        private UserViewModel selectedUser;
        #endregion

        #region Public Properties
        /// <summary>
        /// Użytkownicy w systemie.
        /// </summary>
        public ObservableCollection<UserViewModel> Users
        {
            get { return users; }
            set { this.RaiseAndSetIfChanged(ref users, value); }
        }
        /// <summary>
        /// Wybrany użytkownik.
        /// </summary>
        public UserViewModel SelectedUser
        {
            get { return selectedUser; }
            set { this.RaiseAndSetIfChanged(ref selectedUser, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda dodawania użytkownika.
        /// </summary>
        public ReactiveCommand<object> AddUserCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Komenda usuwania użytkownika.
        /// </summary>
        public ReactiveCommand<object> DeleteUserCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Komenda edycji użytkownika.
        /// </summary>
        public ReactiveCommand<object> EditUserCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy UsersManagementDialogViewModel.
        /// </summary>
        public UsersManagementDialogViewModel()
        {
            usersManagement = ServiceLocator.Resolve<IUsersManagementService>();
            InitUsers();
            InitCommands();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Zwraca użytkownika edytowanego przez operatora.
        /// Null w przypadku anulowania.
        /// </summary>
        /// <returns></returns>
        private async Task<UserViewModel> GetUserFromOperator()
        {
            var userInput = new UserInputDialog()
            {
                //utwórz nową instancję modelu użytkownika w celu uniknięcia wzajemnej edycji w DataGrid
                DataContext = new UserViewModel(selectedUser?.GetUser())
            };

            return await DialogAccess.Show(userInput, RootDialogId) as UserViewModel;
        }
        /// <summary>
        /// Zatwierdzenie zmian dla bieżącego użytkownika.
        /// </summary>
        /// <returns></returns>
        private async Task EditUserMethod()
        {
            var user = await GetUserFromOperator();
            if (user != null)
            {
                usersManagement.UpdateUser(user.GetUser());
                InitUsers();
            }
        }
        /// <summary>
        /// Usunięcie bieżącego użytkownika.
        /// </summary>
        /// <returns></returns>
        private void DeleteUserMethod()
        {
            //zabezpieczenie przed usunięciem samego siebie
            if(selectedUser.Login != MainViewModel.CurrentLoggedUser.Login)
            {
                usersManagement.DeleteUser(selectedUser.Login);
                InitUsers();
            }
        }
        /// <summary>
        /// Dodanie nowego użytkownika.
        /// </summary>
        /// <returns></returns>
        private async Task AddUserMethod()
        {
            SelectedUser = null;
            var user = await GetUserFromOperator();
            if(user != null)
            {
                bool userAlreadyExists = usersManagement.AddUser(user.GetUser());
                if (userAlreadyExists)
                    InitUsers();
                else
                    DialogAccess.ShowToastNotification("Użytkownik o wybranym loginie już istnieje");
            }
        }
        /// <summary>
        /// Inicjalizacja komend.
        /// </summary>
        private void InitCommands()
        {
            //dodawanie użytkownika
            AddUserCommand = ReactiveCommand.Create();
            AddUserCommand.Subscribe(async s => await AddUserMethod());
            //usuwanie użytkownika
            DeleteUserCommand = ReactiveCommand.Create(this.ObservableForProperty(s => s.SelectedUser, user => user != null));
            DeleteUserCommand.Subscribe(s => DeleteUserMethod());
            //edycja użytkownika
            EditUserCommand = ReactiveCommand.Create(this.ObservableForProperty(s => s.SelectedUser, user => user != null));
            EditUserCommand.Subscribe(async s => await EditUserMethod());
        }
        /// <summary>
        /// Incijalizacja listy użytkowników.
        /// </summary>
        private void InitUsers()
        {
            var dbUsers = usersManagement.GetUsers().Select(s => new UserViewModel(s));
            Users = new ObservableCollection<UserViewModel>(dbUsers.OrderBy(s=>s.Login));
            SelectedUser = Users.FirstOrDefault();
        }
        #endregion
    }
}
