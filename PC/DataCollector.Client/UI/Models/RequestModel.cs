using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Model definiujący powiadomienie do użytkownika.
    /// </summary>
    public class RequestModel : MetroDialogSettings, IRequestBuilder
    {
        #region Public Properties
        /// <summary>
        /// Styl dialogu.
        /// </summary>
        public MessageDialogStyle DialogStyle { get; private set; }
        /// <summary>
        /// Wiadomość.
        /// </summary>
        public string Message { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Prywatny konstruktor klasy RequestModel.
        /// </summary>
        private RequestModel()
        {
            this.DialogStyle = MessageDialogStyle.AffirmativeAndNegative;
            this.NegativeButtonText = "Anuluj";
            this.AffirmativeButtonText = "Potwierdź";
            this.FirstAuxiliaryButtonText = "";
            this.SecondAuxiliaryButtonText = "";
        }
        #endregion

        #region Public Methods
        public IRequestBuilder Text(string text)
        {
            this.Message = text;
            return this;
        }
        public IRequestBuilder NegativeButton(string text)
        {
            this.NegativeButtonText = text;
            return this;
        }
        public IRequestBuilder ConfirmButton(string text)
        {
            this.AffirmativeButtonText = text;
            return this;
        }
        public IRequestBuilder ConfirmSecButton(string text)
        {
            this.SecondAuxiliaryButtonText = text;
            return this;
        }
        public IRequestBuilder Style(MessageDialogStyle style)
        {
            this.DialogStyle = style;
            return this;
        }
        public RequestModel Build()
        {
            return this;
        }
        public static IRequestBuilder Create()
        {
            return new RequestModel();
        }
        #endregion

    }
}
