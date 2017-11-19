using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// Implements the basic functionality of presenting a measure data in the view.
    /// </summary>
    public abstract class ChartVisualizationViewModelBase : ViewModelBase
    {
        #region Private Fields
        private string header;
        private long minXAxis, maxXAxis;
        private bool disableAnimations;
        private Func<double, string> xFormatter, yFormatter;
        #endregion

        #region Public Properties
        /// <summary>
        /// The format of the y axis.
        /// </summary>
        public Func<double, string> YFormatter
        {
            get { return yFormatter; }
            set { this.RaiseAndSetIfChanged(ref yFormatter, value); }
        }
        /// <summary>
        /// The header.
        /// </summary>
        public string Header
        {
            get { return header; }
            set { this.RaiseAndSetIfChanged(ref header, value); }
        }
        /// <summary>
        /// Maximum value of x axis.
        /// </summary>
        public long MaxXAxis
        {
            get { return maxXAxis; }
            set { this.RaiseAndSetIfChanged(ref maxXAxis, value); }
        }
        /// <summary>
        /// Minimum value of x axis.
        /// </summary>
        public long MinXAxis
        {
            get { return minXAxis; }
            set { this.RaiseAndSetIfChanged(ref minXAxis, value); }
        }
        /// <summary>
        /// Disable the animations.
        /// </summary>
        public bool DisableAnimations
        {
            get { return disableAnimations; }
            set { this.RaiseAndSetIfChanged(ref disableAnimations, value); }
        }
        /// <summary>
        /// The format of the x axis.
        /// </summary>
        public Func<double, string> XFormatter
        {
            get { return xFormatter; }
            set { this.RaiseAndSetIfChanged(ref xFormatter, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="header">header of the chart</param>
        /// <param name="unit">unit name</param>
        /// <param name="xFormatter">x axis format</param>
        public ChartVisualizationViewModelBase(string header, string unit, Func<double, string> xFormatter)
        {
            this.header = header;

            if (!string.IsNullOrEmpty(unit))
                YFormatter = (val) => $"{Math.Round(val, 4)} {unit}";
            else
                YFormatter = (val) => $"{Math.Round(val, 4)}";

            XFormatter = xFormatter;

            MinXAxis = 0;
            MaxXAxis = 2;
        }
        #endregion
    }
}
