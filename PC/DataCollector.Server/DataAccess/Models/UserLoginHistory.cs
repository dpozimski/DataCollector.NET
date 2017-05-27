using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Klasa reprezentująca pojedyncze zdarzenie logowania użytkownika.
    /// </summary>
    public class UserLoginHistory:BaseTable
    {
        /// <summary>
        /// Czas zalogowania.
        /// </summary>
        [Required]
        public DateTime LoginTimeStamp { get; set; }
        /// <summary>
        /// Czas wylogowania.
        /// </summary>
        public DateTime? LogoutTimeStamp { get; set; }
        /// <summary>
        /// Przypisany użytkownik do zdarzenia.
        /// </summary>
        public virtual User AssignedUser { get; set; }
    }
}
