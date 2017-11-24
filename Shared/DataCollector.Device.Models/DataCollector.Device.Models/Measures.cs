using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Device.Models
{
    /// <summary>
    /// Class which represents a measures collection.
    /// </summary>
    public sealed class Measures
    {
        #region Public Properties
        /// <summary>
        /// Accelrometer value.
        /// </summary>
        public SpherePoint Accelerometer { get; set; }
        /// <summary>
        /// Gyroscope value.
        /// </summary>
        public SpherePoint Gyroscope { get; set; }
        /// <summary>
        /// Humidity [RH].
        /// </summary>
        public float? Humidity { get; set; }
        /// <summary>
        /// Temperature [*C].
        /// </summary>
        public float? Temperature { get; set; }
        /// <summary>
        /// Air pressure [hPa].
        /// </summary>
        public float? AirPressure { get; set; }
        /// <summary>
        /// LED Diode is active.
        /// </summary>
        public bool? IsLedActive { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// Override ToString method to show user-friendly measures text.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{nameof(Accelerometer)}: X={Accelerometer?.X} Y={Accelerometer?.Y}, Z={Accelerometer?.Z}");
            builder.AppendLine($"{nameof(Gyroscope)}    : X={Gyroscope?.X} Y={Gyroscope?.Y}, Z={Gyroscope?.Z}");
            builder.AppendLine($"{nameof(Temperature)}  :   {Temperature}");
            builder.AppendLine($"{nameof(Humidity)}     :   {Humidity}");
            builder.AppendLine($"{nameof(AirPressure)}  :   {AirPressure}");
            builder.AppendLine($"{nameof(IsLedActive)}  :   {(IsLedActive.HasValue && IsLedActive.Value ? "Yes" : "No")}");
            return builder.ToString();
        }
        #endregion
    }
}
