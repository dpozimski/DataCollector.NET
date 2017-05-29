using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Klasa reprezentująca użytkownika.
    /// Na wzór jej tworzymy pozostałe klasy, któe będą zmapowane do tabel w Entity Frmaework.
    /// </summary>
    [DataContract]
    public class User : BaseTable
    {
        #region Public Properties
        /// <summary>
        /// Nazwa użytkownika.
        /// </summary>
        [Required]
        [DataMember]
        public string Login { get; set; }
        /// <summary>
        /// Hasło zakodowane w MD5
        /// </summary>
        [Required]
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// Imię.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Nazwisko.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Uprawnienie.
        /// </summary>
        [Required]
        [DataMember]
        public UserRole Role { get; set; } = UserRole.Viewer;
        /// <summary>
        /// Kolekcja historii logowania użytkownika.
        /// </summary>
        public virtual ICollection<UserLoginHistory> UserLoginHistory { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Metoda przypisująca w zakodowanej formie hasło do danego obiektu użytkownika.
        /// Metody tej należy uzyć dodając nowego użytkownika do bazy danych.
        /// </summary>
        /// <param name="password"></param>
        public void AssignPassword(string password)
        {
            this.Password = EncodePassword(password);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda kodująca hasło do hashu MD5.
        /// </summary>
        /// <param name="password">hasło</param>
        /// <returns></returns>
        private string EncodePassword(string decodedPassword)
        {
            //utworzenie obiektu MD5
            MD5 md5 = MD5.Create();
            //konwersja hasła na tablicę bajtów
            byte[] inputBytes = Encoding.UTF8.GetBytes(decodedPassword);
            //wyliczenie hashu
            byte[] hash = md5.ComputeHash(inputBytes);
            //utworzenie obiektu pozwalającego na łatwiejsze zarządzanie ciągiem znaków
            StringBuilder sb = new StringBuilder();
            //wpisanie wyliczonego hashu do stringa w formacie HEX.
            foreach(byte item in hash)
                sb.Append(item.ToString("X2"));
            //zwrócenie zakodowanego hasła
            return sb.ToString();
        }
        #endregion
    }
}
