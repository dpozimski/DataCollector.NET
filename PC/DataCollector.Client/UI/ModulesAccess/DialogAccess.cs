using DataCollector.Client.Translation;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.Views.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using netoaster;
using netoaster.Enumes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Klasa implementująca obsługę dialogów
    /// </summary>
    class DialogAccess : IDialogAccess
    {
        #region Private Fields
        private MetroWindow hwnd;
        #endregion

        #region Public Methods
        /// <summary>
        /// Referencja do okna głównego aplikacji.
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(MetroWindow hwnd)
        {
            this.hwnd = hwnd;
        }
        /// <summary>
        /// Wyświetla dialog we wskazanym kontenerze.
        /// </summary>
        /// <param name="content">zawartość</param>
        /// <param name="rootDialogId">identyfikator</param>
        /// <returns></returns>
        public async Task<object> Show(UserControl content, string rootDialogId)
        {
            try
            {
                return await DialogHost.Show(content, rootDialogId);
            }
            catch(InvalidOperationException)
            {
                //ignored, because the dialog was openned more than once
                return null;
            }
        }
        /// <summary>
        /// Wyświetla komunikat tekstowy.
        /// </summary>
        /// <param name="message">wiadmość</param>
        /// <returns></returns>
        public async Task ShowMessage(string message)
        {
           await ShowRequestAsync(RequestModel.Create().Text(message).Style(MessageDialogStyle.Affirmative));
        }
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
        public async Task<MessageDialogResult> ShowRequestAsync(IRequestBuilder model)
        {
            RequestModel request = model.Build();
            var result = await hwnd.ShowMessageAsync(hwnd.Title,
                                                                request.Message, request.DialogStyle, request);

            return result;
        }
        /// <summary>
        /// Pobiera dane logowania od użytkownika.
        /// </summary>
        /// <returns></returns>
        public async Task<LoginDialogData> ShowLoginAsync()
        {
            LoginDialogSettings loginDialogSettings = new LoginDialogSettings()
            {
                AffirmativeButtonText = TranslationExtension.GetString("Login"),
                PasswordWatermark = TranslationExtension.GetString("Password"),
                UsernameWatermark = TranslationExtension.GetString("UserName"),
            };
            return await hwnd.ShowLoginAsync(hwnd.Title,
                                TranslationExtension.GetString("PleaseTypeDataToLogIn"), loginDialogSettings);
        }
        /// <summary>
        /// Wyświetla okno oczekiwania.
        /// </summary>
        /// <returns></returns>
        public async Task<ProgressDialogController> GetProgressDialog(string message)
        {
            var settingsDialog = new MetroDialogSettings()
            {
                AnimateShow = false,
                AnimateHide = false,
            };

            return await hwnd.ShowProgressAsync(hwnd.Title, message, settings: settingsDialog);
        }
        /// <summary>
        /// Metoda wyświetlająca powiadomienei w formie dymku.
        /// </summary>
        /// <param name="message">wiadomość</param>
        /// <param name="type">Typ powiadomienia</param>
        public void ShowToastNotification(string message, ToastType type = ToastType.Info)
        {
            switch (type)
            {
                case ToastType.Info:
                    Toaster.ShowInfo(hwnd, hwnd.Title, message);
                    break;
                case ToastType.Error:
                    Toaster.ShowError(hwnd, hwnd.Title, message);
                    break;
                case ToastType.Success:
                    Toaster.ShowSuccess(hwnd, hwnd.Title, message);
                    break;
                case ToastType.Warning:
                    Toaster.ShowWarning(hwnd, hwnd.Title, message);
                    break;
            }
        }
        #endregion
    }
}
