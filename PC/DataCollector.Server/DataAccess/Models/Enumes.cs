using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Rodzaj uprawnień użytkownika.
    /// </summary>
    [Flags]
    public enum UserRole
    {
        /// <summary>
        /// Uprawnienie administratora.
        /// </summary>
        [Description("Administrator")]
        Administrator = 1,
        /// <summary>
        /// Uprawnienie obserwatora.
        /// </summary>
        [Description("Obserwator")]
        Viewer = 2,
        /// <summary>
        /// Uprawnienie obserwatora i administratora.
        /// </summary>
        [Description("Wszyscy")]
        All = Administrator | Viewer
    }
    /// <summary>
    /// Typ pomiaru/
    /// </summary>
    public enum MeasureType
    {
        /// <summary>
        /// Pomiar wigotności w RH.
        /// </summary>
        [MeasureInformationAttribute("Wilgotność", "RH")]
        Humidity,
        /// <summary>
        /// Pomiar wilgotności w Celsjuszach.
        /// </summary>
        [MeasureInformationAttribute("Temperatura", "°C")]
        Temperature,
        /// <summary>
        /// Pomiar ciśnienia atmosferycznego w hPa.
        /// </summary>
        [MeasureInformationAttribute("Ciśnienie", "hPa")]
        AirPressure
    }

    /// <summary>
    /// Rodzaj pomiaru.
    /// </summary>
    public enum SphereMeasureType
    {
        /// <summary>
        /// Żyroskop.
        /// </summary>
        [MeasureInformationAttribute("Żyroskop", "")]
        Gyroscope,
        /// <summary>
        /// Akcelerometr.
        /// </summary>
        [MeasureInformationAttribute("Akcelerometr", "")]
        Accelerometer
    }
}
