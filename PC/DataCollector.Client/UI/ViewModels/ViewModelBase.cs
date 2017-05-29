using DataCollector.Client.UI.Converters;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.ViewModels.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels
{
    /// <summary>
    /// Abstrakcyjna klasa rozszerzająca funckjonalność klasycznego ViewModel o funkcjonalności związane z
    /// wyświetlaniem dialogów.
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject
    {
        #region Private Fields
        private bool isBusy;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Serwis dialogów globalnych.
        /// </summary>
        protected IDialogAccess DialogAccess { get; private set; }
        /// <summary>
        /// Konwerter typów wyliczeiowych na przypisany opis.
        /// </summary>
        protected static EnumToStringDescription EnumStrConverter { get; private set; } = new EnumToStringDescription();
        #endregion

        #region Public Properties
        /// <summary>
        /// Flaga zajętości.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { this.RaiseAndSetIfChanged(ref isBusy, value); }
        }
        /// <summary>
        /// Główny ViewModel aplikacji.
        /// </summary>
        public static ShellViewModel MainViewModel { get; set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DialogViewModelBase.
        /// </summary>
        public ViewModelBase()
        {
            DialogAccess = ServiceLocator.Resolve<IDialogAccess>();
        }
        #endregion
    }
}
