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
        Administrator = 1,
        /// <summary>
        /// Uprawnienie obserwatora.
        /// </summary>
        Viewer = 2,
        /// <summary>
        /// Uprawnienie obserwatora i administratora.
        /// </summary>
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
        Humidity,
        /// <summary>
        /// Pomiar wilgotności w Celsjuszach.
        /// </summary>
        Temperature,
        /// <summary>
        /// Pomiar ciśnienia atmosferycznego w hPa.
        /// </summary>
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
        Gyroscope,
        /// <summary>
        /// Akcelerometr.
        /// </summary>
        Accelerometer
    }
}
