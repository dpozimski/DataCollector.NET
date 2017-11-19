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
    /// The interface which declares a dialog methods.
    /// </summary>
    public interface IDialogAccess
    {
        /// <summary>
        /// Shows a dialog in selected container.
        /// </summary>
        /// <param name="content">content</param>
        /// <param name="rootDialogId">the contianer identifier</param>
        /// <returns></returns>
        Task<object> Show(UserControl content, string rootDialogId);
        /// <summary>
        /// Shows the request asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <CreatedOn>19.11.2017 12:32</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        Task<MessageDialogResult> ShowRequestAsync(IRequestBuilder model);
        /// <summary>
        /// Shows the text message.
        /// </summary>
        /// <param name="message">wiadmość</param>
        /// <returns></returns>
        Task ShowMessage(string message);
        /// <summary>
        /// Shows the login asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <CreatedOn>19.11.2017 12:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        Task<LoginDialogData> ShowLoginAsync();
        /// <summary>
        /// Gets the progress dialog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <CreatedOn>19.11.2017 12:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        Task<ProgressDialogController> GetProgressDialog(string message);
        /// <summary>
        /// Sets the HWND.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <CreatedOn>19.11.2017 12:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        void SetHwnd(MetroWindow shell);
        /// <summary>
        /// Shows the toast notification.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <CreatedOn>19.11.2017 12:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        void ShowToastNotification(string message, ToastType type = ToastType.Info);
    }
}
