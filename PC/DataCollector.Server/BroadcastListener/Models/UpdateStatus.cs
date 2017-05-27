using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener.Models
{
    /// <summary>
    /// Rodzaj aktualizacji urządzenia.
    /// </summary>
    public enum UpdateStatus
    {
        /// <summary>
        /// Urządzenie zostało znalezione.
        /// </summary>
        [Description("Wykryto {0} w sieci")]
        Found,
        /// <summary>
        /// Informacje zostały zaktualizowane.
        /// </summary>
        [Description("Aktualizacja danych - {0}")]
        Updated,
        /// <summary>
        /// Urządzenie nie odpowiada.
        /// </summary>
        [Description("Utracono widoczność {0}")]
        Lost,
        /// <summary>
        /// Połączenie z serwisem webowym.
        /// </summary>
        [Description("Nawiązano komunikację z {0}")]
        ConnectedToRestService,
        /// <summary>
        /// Rozłączenie z serwisemw webowym.
        /// </summary>
        [Description("Rozłączono komunikację z {0}")]
        DisconnectedFromRestService
    }
}
