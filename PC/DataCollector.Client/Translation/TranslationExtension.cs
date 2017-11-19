using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace DataCollector.Client.Translation
{
    /// <summary>
    /// Class provides a functionality to translate a keys using current culture.
    /// </summary>
    public class TranslationExtension
    {
        #region [Public Static Methods]        
        public static string GetString(string key)
        {
            var assembly = typeof(TranslationExtension).Assembly;
            var rm = new ResourceManager("DataCollector.Client.Translation.Strings", assembly);
            var value =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .GetString(key);
            return value;
        }
        #endregion
    }
}
