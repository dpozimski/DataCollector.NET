using DataCollector.Client.UI.Models;
using MahApps.Metro.Controls.Dialogs;
using netoaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollector.Client.UI.Views.Core;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using netoaster.Enumes;

namespace DataCollector.Client.UI.ModulesAccess.Interfaces
{
    /// <summary>
    /// Interfejs implementujący metody obsługi dialogów.
    /// </summary>
    public interface IDialogAccess
    {
        /// <summary>
        /// Wyświetla dialog we wskazanym kontenerze.
        /// </summary>
        /// <param name="content">zawartość</param>
        /// <param name="rootDialogId">identyfikator</param>
        /// <returns></returns>
        Task<object> Show(UserControl content, string rootDialogId);
        /// <summary>
        /// Metoda wyświetlajaća pytanie na ekranie w formie komunikatu..
        /// </summary>
        /// <param name="cancelheader">nagłowek anulowania operacji</param>
        /// <param name="confirmHeader">nagłowęk potwierdzenia operacji</param>
        /// <param name="message">wiadomość</param>
        /// <param name="mode">Określa typ komunikatu.</param>
        /// <param name="firstAuxiliaryHeader">nagłówek przycisku dodatowego</param>
        /// <param name="secondAuxiliaryHeader">nagłówek 2 przycisku dodatkowego</param>
        /// <returns></returns>
        Task<MessageDialogResult> ShowRequestAsync(IRequestBuilder model);
        /// <summary>
        /// Wyświetla komunikat tekstowy.
        /// </summary>
        /// <param name="message">wiadmość</param>
        /// <returns></returns>
        Task ShowMessage(string message);
        /// <summary>
        /// Pobiera dane logowania od użytkownika.
        /// </summary>
        /// <returns></returns>
        Task<LoginDialogData> ShowLoginAsync();
        /// <summary>
        /// Wyświetla okno oczekiwania.
        /// </summary>
        /// <returns></returns>
        Task<ProgressDialogController> GetProgressDialog(string message);
        /// <summary>
        /// Referencja do okna głównego aplikacji.
        /// </summary>
        /// <param name="shell"></param>
        void SetHwnd(MetroWindow shell);
        /// <summary>
        /// Metoda wyświetlająca powiadomienei w formie dymku.
        /// </summary>
        /// <param name="message">wiadomość</param>
        /// <param name="type">Typ powiadomienia</param>
        void ShowToastNotification(string message, ToastType type = ToastType.Info);
    }
}
