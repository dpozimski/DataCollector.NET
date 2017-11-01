using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models.Entities
{
    /// <summary>
    /// Klasa abstrakcyjna reprezentująca indeksowaną tabelę.
    /// </summary>
    [DataContract]
    public abstract class BaseTable
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int ID { get; set; }
    }
}
