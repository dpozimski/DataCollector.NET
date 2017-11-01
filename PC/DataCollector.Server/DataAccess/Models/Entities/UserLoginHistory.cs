using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models.Entities
{
    /// <summary>
    /// Klasa reprezentująca pojedyncze zdarzenie logowania użytkownika.
    /// </summary>
    [DataContract]
    public class UserLoginHistory:BaseTable
    {
        /// <summary>
        /// Czas zalogowania.
        /// </summary>
        [Required]
        [DataMember]
        public DateTime LoginTimeStamp { get; set; }
        /// <summary>
        /// Czas wylogowania.
        /// </summary>
        [DataMember]
        public DateTime? LogoutTimeStamp { get; set; }
        /// <summary>
        /// Przypisany użytkownik do zdarzenia.
        /// </summary>
        public virtual User AssignedUser { get; set; }
    }
}
