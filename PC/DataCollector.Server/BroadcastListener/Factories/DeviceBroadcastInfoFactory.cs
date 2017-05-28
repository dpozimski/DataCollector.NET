using DataCollector.Server.BroadcastListener.Exceptions;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener.Factories
{
    /// <summary>
    /// Fabryka obiektów reprezentująych ramkę identyfikacyjną.
    /// </summary>
    public class DeviceBroadcastInfoFactory : IDevicesBroadcastInfoFactory
    {
        /// <summary>
        /// Oczekiwana długość ramki.
        /// </summary>
        private const int FrameLength = 446;
        /// <summary>
        /// Oczekiwana liczba bloków w pakiecie.
        /// </summary>
        private const byte BlockCount = 7;
        /// <summary>
        /// Oczekiwana liczba znaków w identyfikatorze MAC.
        /// </summary>
        private const byte MacAddressSignCount = 6;

        /// <summary>
        /// Metoda parsująca bufor wejściowy do obiektu Device.
        /// </summary>
        /// <param name="buffer">bufor wejściowy</param>
        /// <param name="device">informacje o urządzeniu</param>
        /// <returns>obiekt definiujący powstałą ramkę</returns>
        public IDeviceBroadcastInfo From(byte[] buffer)
        {
            if (buffer is null)
                throw new ArgumentNullException();

            if (buffer.Length != FrameLength)
                throw new InvalidFrameException(InvalidFrameException.ErrorType.FrameSize, $"Expected {FrameLength} bytes.");

            string text = Encoding.Unicode.GetString(buffer);

            string[] parts = text.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != BlockCount)
                throw new InvalidFrameException(InvalidFrameException.ErrorType.BlockCount, $"Expected {BlockCount} blocks.");

            string name = parts[0];

            IPAddress ipv4 = IPAddress.Parse(parts[1]);

            string macAddressString = parts[2];
            string[] macAddressByteStrings = macAddressString.Split(':');
            if (macAddressByteStrings.Length != MacAddressSignCount)
                throw new InvalidFrameException(InvalidFrameException.ErrorType.MacAddressBadFormat, $"Expected {MacAddressSignCount} digits.");

            string deviceModel = parts[4];
            string winver = parts[5];
            string architecture = parts[6];

            return new DeviceBroadcastInfo(name, ipv4, macAddressString, architecture, winver, deviceModel);
        }
    }
}
