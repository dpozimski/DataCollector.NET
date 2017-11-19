using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.Users;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataCollector.Client.UI.ViewModels.Core
{
    /// <summary>
    /// The view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    public class UserViewModel : ViewModelBase
    {
        #region Private Fields
        private IUsersManagementService managementService;
        private int sessionId;
        private User user;
        private bool logoutRequested;
        private bool isPasswordDirty;
        private ObservableCollection<UserRole> availableRoles;
        private ObservableCollection<UserLoginHistory> loginHistory;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets a value indicating whether [logout requested].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [logout requested]; otherwise, <c>false</c>.
        /// </value>
        public bool LogoutRequested
        {
            get { return logoutRequested; }
            set
            {
                logoutRequested = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public int SessionId
        {
            get { return sessionId; }
            set { this.RaiseAndSetIfChanged(ref sessionId, value); }
        }
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login
        {
            get { return user.Login; }
            set
            {
                user.Login = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return user.Password; }
            set { user.Password = value;
                isPasswordDirty = true;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get { return user.FirstName; }
            set
            {
                user.FirstName = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get { return user.LastName; }
            set
            {
                user.LastName = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public UserRole Role
        {
            get { return user.Role; }
            set
            {
                user.Role = value;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the available roles.
        /// </summary>
        /// <value>
        /// The available roles.
        /// </value>
        public ObservableCollection<UserRole> AvailableRoles
        {
            get { return availableRoles; }
            set { this.RaiseAndSetIfChanged(ref availableRoles, value); }
        }
        /// <summary>
        /// Gets or sets the login history.
        /// </summary>
        /// <value>
        /// The login history.
        /// </value>
        public ObservableCollection<UserLoginHistory> LoginHistory
        {
            get { return loginHistory; }
            set { this.RaiseAndSetIfChanged(ref loginHistory, value); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the logout command.
        /// </summary>
        /// <value>
        /// The logout command.
        /// </value>
        public ReactiveCommand<object> LogoutCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserViewModel(User user):this(new UserSession() { SessionUser = user, SessionId = -1 })
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userSession">The user session.</param>
        public UserViewModel(UserSession userSession)
        {
            //the roles initialization
            var roles = ((UserRole[])Enum.GetValues(typeof(UserRole))).Where(s => s != UserRole.All);
            this.AvailableRoles = new ObservableCollection<UserRole>(roles);

            //updates the user data
            Update(userSession.SessionUser);

            //assign a session id
            SessionId = userSession.SessionId;

            //the logout command init
            LogoutCommand = ReactiveCommand.Create();
            LogoutCommand.Subscribe(s => LogoutRequested = !LogoutRequested);

            //gets the service
            managementService = ServiceLocator.Resolve<IUsersManagementService>();
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Fills the login history of the current user.
        /// </summary>
        public void FillLoginHistory()
        {
            //Get the login history collection
            var userLoginHistory = managementService.GetUserLoginHistory(user);
            LoginHistory = new ObservableCollection<UserLoginHistory>(userLoginHistory);
        }
        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Update(User user)
        {
            this.user = user ?? new User();
            this.RaisePropertyChanged(null);
        }
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {
            var user = new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Login = this.user.Login,
                Role = Role,
                ID = this.user.ID
            };

            if (isPasswordDirty)
                user.Password = Password;
            else
                user.Password = Password;

            return user;
        }
        #endregion
    }
}
