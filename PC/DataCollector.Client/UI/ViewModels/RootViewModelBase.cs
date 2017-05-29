using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels
{
    public class RootViewModelBase : ViewModelBase
    {
        #region Private Fields
        private string rootDialogId;
        #endregion

        #region Public Properties
        /// <summary>
        /// Identyfikator dialogów wewnętrznych.
        /// Wymaga implementacji w widoku.
        /// <wpf:DialogHost></Dialog>
        /// </summary>
        public string RootDialogId
        {
            get { return rootDialogId; }
            set { this.RaiseAndSetIfChanged(ref rootDialogId, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy RootViewModelBase.
        /// </summary>
        public RootViewModelBase()
        {
            RootDialogId = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
