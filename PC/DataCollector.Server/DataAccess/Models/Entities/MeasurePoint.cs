using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models.Entities
{
    /// <summary>
    /// Klasa reprezentująca pojedynczy pomiar w przestrzeni.
    /// </summary>
    public class MeasurePoint : BaseTable
    {
        /// <summary>
        /// Wartość.
        /// </summary>
        public float? Value { get; set; }
        /// <summary>
        /// Typ.
        /// </summary>
        [Required]
        public MeasureType Type { get; set; }
        /// <summary>
        /// Referencja do właściciela pomiaru.
        /// </summary>
        public virtual DeviceTimeMeasurePoint AssignedDeviceMeasureTimePoint { get; set; }
    }
}
