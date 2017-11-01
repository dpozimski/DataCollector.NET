using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollector.Device.Models;

namespace DataCollector.Server.DataAccess.Models.Entities
{
    /// <summary>
    /// Klasa reprezentująca pomiar punktu w przestrzeni.
    /// </summary>
    public class SphereMeasurePoint : BaseTable
    {
        /// <summary>
        /// Punkt w przestrzeni.
        /// </summary>
        public SpherePoint Point { get; set; }
        /// <summary>
        /// Typ pomiaru.
        /// </summary>
        [Required]
        public SphereMeasureType Type { get; set; }
        /// <summary>
        /// Referencja do właściciela pomiaru.
        /// </summary>
        public virtual DeviceTimeMeasurePoint AssignedDeviceMeasureTimePoint { get; set; }
    }
}
