using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Exceptions
{
    /// <summary>
    /// Klasa reprezentująca wyjątek powstały w czasie parsowania ramki.
    /// </summary>
    public class InvalidFrameException : Exception
    {
        public enum ErrorType
        {
            /// <summary>
            /// Zła długość ramki wejściowej.
            /// </summary>
            FrameSize,
            /// <summary>
            /// Nieprawidłowa liczba bloków w pakiecie.
            /// </summary>
            BlockCount,
            /// <summary>
            /// Nieprawidłowy format adresu MAC.
            /// </summary>
            MacAddressBadFormat
        }

        /// <summary>
        /// Typ błędu wskazany przez wyjątek.
        /// </summary>
        public ErrorType Type { get; }

        /// <summary>
        /// Tworzy nową instancję klasy.
        /// </summary>
        /// <param name="type">typ błędu</param>
        /// <param name="message">Wiadomość dodatkowa</param>
        public InvalidFrameException(ErrorType type, string message = null)
            : base(message)
        {
            this.Type = type;
        }
    }
}
