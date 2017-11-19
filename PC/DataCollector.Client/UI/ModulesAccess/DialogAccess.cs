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
    /// Class which implements a dialog access.
    /// </summary>
    class DialogAccess : IDialogAccess
    {
        #region Private Fields
        private MetroWindow hwnd;
        #endregion

        #region Public Methods        
        /// <summary>
        /// Sets the HWND.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        public void SetHwnd(MetroWindow hwnd)
        {
            this.hwnd = hwnd;
        }
        /// <summary>
        /// Shows a dialog in selected container.
        /// </summary>
        /// <param name="content">content</param>
        /// <param name="rootDialogId">the contianer identifier</param>
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
        /// Shows the text message.
        /// </summary>
        /// <param name="message">wiadmość</param>
        /// <returns></returns>
        public async Task ShowMessage(string message)
        {
           await ShowRequestAsync(RequestModel.Create().Text(message).Style(MessageDialogStyle.Affirmative));
        }
        /// <summary>
        /// Shows the request asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<MessageDialogResult> ShowRequestAsync(IRequestBuilder model)
        {
            RequestModel request = model.Build();
            var result = await hwnd.ShowMessageAsync(hwnd.Title,
                                                                request.Message, request.DialogStyle, request);

            return result;
        }
        /// <summary>
        /// Shows the login asynchronous.
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
        /// Gets the progress dialog.
        /// </summary>
        /// <param name="message">The message.</param>
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
        /// Shows the toast notification.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <CreatedOn>19.11.2017 12:37</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
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
