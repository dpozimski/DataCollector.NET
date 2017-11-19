using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels
{
    /// <summary>
    /// A base class for all view model which adds the window reference id.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    public class RootViewModelBase : ViewModelBase
    {
        #region Private Fields
        private string rootDialogId;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets the root dialog identifier.
        /// </summary>
        /// <value>
        /// The root dialog identifier.
        /// </value>
        public string RootDialogId
        {
            get { return rootDialogId; }
            set { this.RaiseAndSetIfChanged(ref rootDialogId, value); }
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="RootViewModelBase"/> class.
        /// </summary>
        public RootViewModelBase()
        {
            RootDialogId = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
