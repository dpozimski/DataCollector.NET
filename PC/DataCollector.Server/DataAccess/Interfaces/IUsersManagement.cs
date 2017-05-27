using DataCollector.Server.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metody zarządzania użytkownikami.
    /// </summary>
    public interface IUsersManagement : IDataAccessBase
    {
        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Rejestracja zdarzenia wylogowania się użytkownika z bieżącej sesji.
        /// </summary>
        /// <param name="sessionId">identyfikaor sesji</param>
        void RecordLogoutTimeStamp(int sessionId);
        /// <summary>
        /// Zwraca historię logowania użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IReadOnlyList<UserLoginHistory> GetUserLoginHistory(User user);
        /// <summary>
        /// Weryfikacja poświadczeń użytkownika.
        /// </summary>
        /// <param name="username">login</param>
        /// <param name="password">hasło</param>
        /// <returns></returns>
        bool ValidateCredentials(string username, string password);
        /// <summary>
        /// Metoda zwracająca użytkownika ze zgodnymi danymi w argumentach
        /// lub null jeśli dane są niepoprawne.
        /// </summary>
        /// <param name="username">login</param>
        /// <returns>użytkownik z identyfikatorem sesji</returns>
        Tuple<User, int> GetUser(string username);
        /// <summary>
        /// Metoda zwracająca listę wszystkich użytkowników w bazie danych.
        /// </summary>
        /// <returns>lista użytkowników</returns>
        IReadOnlyList<User> GetUsers();
        /// <summary>
        /// Metoda dodająca nowego użytkownika.
        /// <param name="user">użytkownik</param>
        /// <returns>zwraca raport z przeprowadzonej operacji</returns>
        bool AddUser(User user);
        /// <summary>
        /// Metoda usuwająca użytkownika o danym loginie z bazy danych.
        /// </summary>
        /// <param name="login">login</param>
        /// <returns>sukces</returns>
        bool DeleteUser(string login);
        /// <summary>
        /// Metoda aktualizująca dane istniejącego użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>sukces</returns>
        void UpdateUser(User user);
        #endregion
    }
}
