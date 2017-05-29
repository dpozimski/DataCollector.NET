using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// Abstrakcyjna klasa definiująca podstawowy ViewModel prezentacji danych wizualizacyjnych.
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
        /// Firmatowanie osi Y.
        /// </summary>
        public Func<double, string> YFormatter
        {
            get { return yFormatter; }
            set { this.RaiseAndSetIfChanged(ref yFormatter, value); }
        }
        /// <summary>
        /// Nagłówek pomiaru.
        /// </summary>
        public string Header
        {
            get { return header; }
            set { this.RaiseAndSetIfChanged(ref header, value); }
        }
        /// <summary>
        /// Maksymalna wartość osi X wykresu w czasie rzeczywistym.
        /// </summary>
        public long MaxXAxis
        {
            get { return maxXAxis; }
            set { this.RaiseAndSetIfChanged(ref maxXAxis, value); }
        }
        /// <summary>
        /// Minimalna wartość osi X wykresu w czasie rzeczywistym.
        /// </summary>
        public long MinXAxis
        {
            get { return minXAxis; }
            set { this.RaiseAndSetIfChanged(ref minXAxis, value); }
        }
        /// <summary>
        /// Dezaktywacja animacji.
        /// Oszczędzanie zasobów aplikacji.
        /// </summary>
        public bool DisableAnimations
        {
            get { return disableAnimations; }
            set { this.RaiseAndSetIfChanged(ref disableAnimations, value); }
        }
        /// <summary>
        /// Sposób formatowania osi X wykresu w czasie rzeczywistym.
        /// </summary>
        public Func<double, string> XFormatter
        {
            get { return xFormatter; }
            set { this.RaiseAndSetIfChanged(ref xFormatter, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy ChartVisualizationViewModelBase.
        /// </summary>
        /// <param name="header">Nagłówek wykresu</param>
        /// <param name="unit">jednostka pomiarowa</param>
        /// <param name="xFormatter">formatowanie osi x</param>
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
