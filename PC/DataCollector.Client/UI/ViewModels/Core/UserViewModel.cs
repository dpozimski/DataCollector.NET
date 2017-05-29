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
    /// ViewModel implementujący modyfikację danych użytkownika.
    /// </summary>
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
        /// Flaga żądania wylogowania sie z aplikacji,
        /// </summary>
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
        /// Identyfikator trwającej sesji.
        /// </summary>
        public int SessionId
        {
            get { return sessionId; }
            set { this.RaiseAndSetIfChanged(ref sessionId, value); }
        }
        /// <summary>
        /// Nazwa użytkownika.
        /// </summary>
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
        /// Hasło zakodowane w MD5
        /// </summary>
        public string Password
        {
            get { return user.Password; }
            set { user.Password = value;
                isPasswordDirty = true;
                this.RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Imię.
        /// </summary>
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
        /// Nazwisko.
        /// </summary>
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
        /// Uprawnienie.
        /// </summary>
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
        /// Dostępne uprawnienia.
        /// </summary>
        public ObservableCollection<UserRole> AvailableRoles
        {
            get { return availableRoles; }
            set { this.RaiseAndSetIfChanged(ref availableRoles, value); }
        }
        /// <summary>
        /// Historia logowania użytkownika.
        /// </summary>
        public ObservableCollection<UserLoginHistory> LoginHistory
        {
            get { return loginHistory; }
            set { this.RaiseAndSetIfChanged(ref loginHistory, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda wylogowania uzytkownika.
        /// </summary>
        public ReactiveCommand<object> LogoutCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy UserViewModel
        /// </summary>
        /// <param name="user">użytkownik</param>
        public UserViewModel(User user):this(new Tuple<User, int>(user, -1))
        {

        }
        /// <summary>
        /// Konstruktor klasy UserViewModel.
        /// </summary>
        /// <param name="user">użytkownik bazodanowy z identyfikatorem sesji</param>
        public UserViewModel(Tuple<User, int> user)
        {
            //inicjalizacja uprawnień
            var roles = ((UserRole[])Enum.GetValues(typeof(UserRole))).Where(s => s != UserRole.All);
            this.AvailableRoles = new ObservableCollection<UserRole>(roles);

            //aktualizacja danych uzytkownika
            Update(user.Item1);

            //przypisanie id sesji
            this.SessionId = user.Item2;

            //inicjalizacja komendy wylogowywania się
            LogoutCommand = ReactiveCommand.Create();
            LogoutCommand.Subscribe(s => LogoutRequested = !LogoutRequested);

            //pobranie danych logowania
            managementService = ServiceLocator.Resolve<IUsersManagementService>();
            LoginHistory = new ObservableCollection<UserLoginHistory>(managementService.GetUserLoginHistory(user.Item1));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Aktualizuje ViewModel o nowe dane.
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            this.user = user ?? new User();
            this.RaisePropertyChanged(null);
        }
        /// <summary>
        /// Zwraca nowo utworzonego obiektu na podstawie wlaściwości klasy.
        /// </summary>
        /// <returns>użytkownik z identyfikatorem sesji</returns>
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
                user.Password = Password;//user.AssignPassword(Password);
            else
                user.Password = Password;

            return user;
        }
        #endregion
    }
}
