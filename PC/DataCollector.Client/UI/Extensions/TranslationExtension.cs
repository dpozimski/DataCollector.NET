using DataCollector.Client.UI.Resources.Translation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace DataCollector.Client.UI.Extensions
{
    /// <summary>
    /// Class provides a functionality to translate a keys using current culture.
    /// </summary>
    /// <CreatedOn>01.11.2017 21:17</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    public class TranslationExtension : MarkupExtension
    {
        #region [Public Properties]
        public string Key { get; set; }
        #endregion

        #region [Public Static Methods]        
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <CreatedOn>01.11.2017 21:20</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public static string GetString(string key)
        {
            var assembly = typeof(TranslationExtension).Assembly;
            var rm = new ResourceManager("DataCollector.Client.UI.Resources.Translation.DataCollectorStrings", assembly);
            var value =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .GetString(key);
            return value;
        }
        #endregion

        #region [Overrides]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return GetString(Key);
        }
        #endregion
    }
}
