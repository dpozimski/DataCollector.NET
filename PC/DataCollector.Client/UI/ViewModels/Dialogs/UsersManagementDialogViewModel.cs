using DataCollector.Client.Translation;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.Users;
using DataCollector.Client.UI.ViewModels.Core;
using DataCollector.Client.UI.Views.Core;
using DataCollector.Client.UI.Views.Dialogs;
using LiveCharts.Helpers;
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
    /// The management dialog view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.RootViewModelBase" />
    class UsersManagementDialogViewModel : RootViewModelBase
    {
        #region Private Fields
        private IUsersManagementService usersManagement;
        private ObservableCollection<UserViewModel> users;
        private UserViewModel selectedUser;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ObservableCollection<UserViewModel> Users
        {
            get { return users; }
            set { this.RaiseAndSetIfChanged(ref users, value); }
        }
        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        /// <value>
        /// The selected user.
        /// </value>
        public UserViewModel SelectedUser
        {
            get { return selectedUser; }
            set { this.RaiseAndSetIfChanged(ref selectedUser, value); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the add user command.
        /// </summary>
        /// <value>
        /// The add user command.
        /// </value>
        public ReactiveCommand<object> AddUserCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Gets or sets the delete user command.
        /// </summary>
        /// <value>
        /// The delete user command.
        /// </value>
        public ReactiveCommand<object> DeleteUserCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Gets or sets the edit user command.
        /// </summary>
        /// <value>
        /// The edit user command.
        /// </value>
        public ReactiveCommand<object> EditUserCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersManagementDialogViewModel"/> class.
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
        /// Gets the user from operator.
        /// </summary>
        /// <returns></returns>
        private async Task<UserViewModel> GetUserFromOperator()
        {
            var userInput = new UserInputDialog()
            {
                //creates a new instance of the user to prevent from double edit in DataGrid
                DataContext = new UserViewModel(selectedUser?.GetUser())
            };

            return await DialogAccess.Show(userInput, RootDialogId) as UserViewModel;
        }
        /// <summary>
        /// Edits the user method.
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
        /// Deletes the user method.
        /// </summary>
        /// <CreatedOn>19.11.2017 14:32</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        private void DeleteUserMethod()
        {
            //do not allow to delete the same user as its logged to the app
            if(selectedUser.Login != MainViewModel.CurrentLoggedUser.Login)
            {
                usersManagement.DeleteUser(selectedUser.Login);
                InitUsers();
            }
        }
        /// <summary>
        /// Adds the user method.
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
                    DialogAccess.ShowToastNotification(TranslationExtension.GetString("UserNameIsTaken"));
            }
        }
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            AddUserCommand = ReactiveCommand.Create();
            AddUserCommand.Subscribe(async s => await AddUserMethod());
            DeleteUserCommand = ReactiveCommand.Create(this.ObservableForProperty(s => s.SelectedUser, user => user != null));
            DeleteUserCommand.Subscribe(s => DeleteUserMethod());
            EditUserCommand = ReactiveCommand.Create(this.ObservableForProperty(s => s.SelectedUser, user => user != null));
            EditUserCommand.Subscribe(async s => await EditUserMethod());
        }
        /// <summary>
        /// Initializes the users.
        /// </summary>
        /// <CreatedOn>19.11.2017 14:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        private void InitUsers()
        {
            var dbUsers = usersManagement.GetUsers().Select(s => new UserViewModel(s));
            Users = new ObservableCollection<UserViewModel>(dbUsers.OrderBy(s=>s.Login));
            Users.ForEach(s => s.FillLoginHistory());
            SelectedUser = Users.FirstOrDefault();
        }
        #endregion
    }
}
