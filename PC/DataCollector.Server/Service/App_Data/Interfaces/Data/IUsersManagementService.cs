using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces.Data
{
    /// <summary>
    /// Interfejs zawierający metody zarządzania użytkownikami.
    /// </summary>
    [ServiceContract]
    public interface IUsersManagementService : IDataAccessBase
    {
        #region Methods
        /// <summary>
        /// Rejestracja zdarzenia wylogowania się użytkownika z bieżącej sesji.
        /// </summary>
        /// <param name="sessionId">identyfikaor sesji</param>
        [OperationContract]
        void RecordLogoutTimeStamp(int sessionId);
        /// <summary>
        /// Zwraca historię logowania użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        List<UserLoginHistory> GetUserLoginHistory(User user);
        /// <summary>
        /// Weryfikacja poświadczeń użytkownika.
        /// </summary>
        /// <param name="username">login</param>
        /// <param name="password">hasło</param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateCredentials(string username, string password);
        /// <summary>
        /// Metoda zwracająca użytkownika ze zgodnymi danymi w argumentach
        /// lub null jeśli dane są niepoprawne.
        /// </summary>
        /// <param name="username">login</param>
        /// <returns>użytkownik z identyfikatorem sesji</returns>
        [OperationContract]
        Tuple<User, int> GetUser(string username);
        /// <summary>
        /// Metoda zwracająca listę wszystkich użytkowników w bazie danych.
        /// </summary>
        /// <returns>lista użytkowników</returns>
        [OperationContract]
        List<User> GetUsers();
        /// <summary>
        /// Metoda dodająca nowego użytkownika.
        /// <param name="user">użytkownik</param>
        /// <returns>zwraca raport z przeprowadzonej operacji</returns>
        [OperationContract]
        bool AddUser(User user);
        /// <summary>
        /// Metoda usuwająca użytkownika o danym loginie z bazy danych.
        /// </summary>
        /// <param name="login">login</param>
        /// <returns>sukces</returns>
        [OperationContract]
        bool DeleteUser(string login);
        /// <summary>
        /// Metoda aktualizująca dane istniejącego użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>sukces</returns>
        [OperationContract]
        void UpdateUser(User user);
        #endregion
    }
}
