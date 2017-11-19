using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Class which represents the single object of the column chart.
    /// </summary>
    public class SeriesModel : IObservableChartPoint
    {
        #region Private Fields
        private double value;
        private string name;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the value that indicates whether
        /// the first value was added to the series.
        /// </summary>
        public bool IsTouched { get; private set; }
        /// <summary>
        /// The value.
        /// </summary>
        public double Value
        {
            get { return value; }
            set { this.value = value;
                IsTouched = true;
                OnPointChanged();
            }
        }
        /// <summary>
        /// The column name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value;
                OnPointChanged();
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="name">Name of the series</param>
        public SeriesModel(string name)
        {
            this.Name = name;
        }
        #endregion

        #region [IObservableChartPoint]
        public event Action PointChanged;
        protected void OnPointChanged()
        {
            PointChanged?.Invoke();
        }
        #endregion
    }
}
