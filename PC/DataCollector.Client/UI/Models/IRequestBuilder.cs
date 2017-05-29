using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Definicja metod buildera powiadomienia dla użytkownika.
    /// </summary>
    public interface IRequestBuilder
    {
        IRequestBuilder Text(string text);
        IRequestBuilder NegativeButton(string text);
        IRequestBuilder ConfirmButton(string text);
        IRequestBuilder ConfirmSecButton(string text);
        IRequestBuilder Style(MessageDialogStyle style);
        RequestModel Build();
    }
}
