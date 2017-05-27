using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Klasa reprezentująca tabelę z odciskiem czasu dla danego urządzenia.
    /// </summary>
    public class DeviceTimeMeasurePoint : BaseTable
    {
        /// <summary>
        /// Odcisk czasu.
        /// </summary>
        [Required]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Kolekcja pomiarów jednowymiarowych.
        /// </summary>
        public virtual ICollection<MeasurePoint> MeasurePoints { get; set; }
        /// <summary>
        /// Kolekcja pomiarów przestrzennych.
        /// </summary>
        public virtual ICollection<SphereMeasurePoint> SphereMeasurePoints { get; set; }
        /// <summary>
        /// Przynależące urządzenie pomiarowe.
        /// </summary>
        public virtual MeasureDevice AssignedMeasureDevice { get; set; }
    }
}
