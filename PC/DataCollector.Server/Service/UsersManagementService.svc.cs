using DataCollector.Server.DataAccess;
using DataCollector.Server.DataAccess.Context;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DataAccess.Models.Entities;
using DataCollector.Server.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server
{
    /// <summary>
    /// Klasa implementująca interfejs IUseranagement.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class UsersManagementService : DataAccessBase, IUsersManagementService
    {
        #region ctor
        /// <summary>
        /// Konstruktor nowej instancji klasy.
        /// </summary>
        /// <param name="ConnectionString">dane połączeniowe</param>
        public UsersManagementService(string ConnectionString) : base(ConnectionString)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Rejestracja zdarzenia wylogowania się użytkownika z bieżącej sesji.
        /// </summary>
        /// <param name="sessionId">identyfikaor sesji</param>
        public void RecordLogoutTimeStamp(int sessionId)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                var session = db.UsersLoginHistory.Single(s => s.ID == sessionId);
                session.LogoutTimeStamp = DateTime.Now;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Weryfikacja poświadczeń użytkownika.
        /// </summary>
        /// <param name="username">login</param>
        /// <param name="password">hasło</param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                User tmpUser = new User();
                tmpUser.Login = username;
                tmpUser.AssignPassword(password);
                return db.Users.Any(s => tmpUser.Login == s.Login && tmpUser.Password == s.Password);
            }
        }
        /// <summary>
        /// Metoda zwracająca użytkownika ze zgodnymi danymi w argumentach
        /// lub null jeśli dane są niepoprawne.
        /// </summary>
        /// <param name="username">login</param>
        /// <returns>użytkownik z identyfikatorem sesji</returns>
        public UserSession GetUser(string username)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                var user = db.Users.SingleOrDefault(s => s.Login == username);
                var session = new UserLoginHistory() { AssignedUser = user, LoginTimeStamp = DateTime.Now };
                if (user != null)
                {
                    //dodanie zapisu o pobraniu użytkownika z bazy danych
                    db.UsersLoginHistory.Add(session);
                    db.SaveChanges();
                }
                return new UserSession() { SessionUser = user, SessionId = session.ID };
            }
        }
        /// <summary>
        /// Metoda zwracająca listę wszystkich użytkowników w bazie danych.
        /// </summary>
        /// <returns>lista użytkowników</returns>
        public List<User> GetUsers()
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                var users = db.Users.Include("UserLoginHistory").ToList();
                return users;
            }
        }
        /// <summary>
        /// Zwraca historię logowania użytkownika.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserLoginHistory> GetUserLoginHistory(User user)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                return db.UsersLoginHistory.Where(s => s.AssignedUser.Login == user.Login).ToList();
            }
        }
        /// <summary>
        /// Metoda dodająca nowego użytkownika.
        /// <param name="user">użytkownik</param>
        /// <returns>zwraca raport z przeprowadzonej operacji</returns>
        public bool AddUser(User user)
        {
            bool success = false;
            using (var db = new DataCollectorContext(ConnectionString))
            {
                if (!db.Users.Any(s => s.Login == user.Login))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    success = true;
                }
            }
            return success;
        }
        /// <summary>
        /// Metoda usuwająca użytkownika o danym loginie z bazy danych.
        /// </summary>
        /// <param name="login">login</param>
        /// <returns>sukces</returns>
        public bool DeleteUser(string login)
        {
            bool succes = false;
            using (var db = new DataCollectorContext(ConnectionString))
            {
                var user = db.Users.Include("UserLoginHistory").SingleOrDefault(s => s.Login == login);
                if (user != null)
                {
                    //usuniecie historii logowania
                    db.UsersLoginHistory.RemoveRange(user.UserLoginHistory);
                    //usunięcie użytkownika
                    db.Users.Remove(user);
                    db.SaveChanges();
                    succes = true;
                }
            }
            return succes;
        }
        /// <summary>
        /// Metoda aktualizująca dane istniejącego użytkownika.
        /// Login musi być stały.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>sukces</returns>
        public void UpdateUser(User user)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                var existingUser = db.Users.Single(s => s.Login == user.Login);
                existingUser.Password = user.Password;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Role = user.Role;
                db.SaveChanges();
            }
        }
        #endregion
    }
}
