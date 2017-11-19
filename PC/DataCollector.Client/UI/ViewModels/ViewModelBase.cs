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
    /// A base class for all model.
    /// </summary>
    /// <seealso cref="ReactiveUI.ReactiveObject" />
    public abstract class ViewModelBase : ReactiveObject
    {
        #region Private Fields
        private bool isBusy;
        #endregion

        #region Protected Properties        
        /// <summary>
        /// Gets the dialog access.
        /// </summary>
        /// <value>
        /// The dialog access.
        /// </value>
        protected IDialogAccess DialogAccess { get; private set; }
        /// <summary>
        /// Gets the enum string converter.
        /// </summary>
        /// <value>
        /// The enum string converter.
        /// </value>
        protected static EnumToStringDescription EnumStrConverter { get; private set; } = new EnumToStringDescription();
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get { return isBusy; }
            set { this.RaiseAndSetIfChanged(ref isBusy, value); }
        }
        /// <summary>
        /// Gets or sets the main view model.
        /// </summary>
        /// <value>
        /// The main view model.
        /// </value>
        public static ShellViewModel MainViewModel { get; set; }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        public ViewModelBase()
        {
            DialogAccess = ServiceLocator.Resolve<IDialogAccess>();
        }
        #endregion
    }
}
