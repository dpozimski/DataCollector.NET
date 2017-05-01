using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Device.Models
{
    /// <summary>
    /// Klasa zrzeszająca pomiary pobrane z źródła zewnętrznego.
    /// </summary>
    public sealed class Measures
    {
        #region Public Properties
        /// <summary>
        /// Akcelerometr.
        /// </summary>
        public SpherePoint Accelerometer { get; set; }
        /// <summary>
        /// Żyroskop.
        /// </summary>
        public SpherePoint Gyroscope { get; set; }
        /// <summary>
        /// Wilgotność [RH].
        /// </summary>
        public float? Humidity { get; set; }
        /// <summary>
        /// Temperatura [*C].
        /// </summary>
        public float? Temperature { get; set; }
        /// <summary>
        /// Ciśnienie atmosferyczne [hPa].
        /// </summary>
        public float? AirPressure { get; set; }
        /// <summary>
        /// Dioda LED jest aktywna.
        /// </summary>
        public bool? IsLedActive { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// Metoda nadpisująca metodę ToString w celu wyświetlenia zawartości obiektu.
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
            builder.AppendLine($"{nameof(IsLedActive)}  :   {(IsLedActive.HasValue && IsLedActive.Value ? "Tak" : "Nie")}");
            return builder.ToString();
        }
        #endregion
    }
}
