using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Klasa reprezentująca pojedynczy obiekt wykresu kolumnowego.
    /// </summary>
    public class SeriesModel : IObservableChartPoint
    {
        #region Private Fields
        private double value;
        private string name;
        #endregion

        #region Public Properties
        /// <summary>
        /// Definiuje czy w danym obiekcie dokonano zmiany pierwotnej wartości.
        /// </summary>
        public bool IsTouched { get; private set; }
        /// <summary>
        /// Wartość.
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
        /// Nazwa kolumny.
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
        /// Konstruktor klasy SeriesModel.
        /// </summary>
        /// <param name="name">Nazwa pola</param>
        public SeriesModel(string name)
        {
            this.Name = name;
        }
        #endregion

        #region IObservableChartPoint
        public event Action PointChanged;
        protected void OnPointChanged()
        {
            if (PointChanged != null) PointChanged.Invoke();
        }
        #endregion
    }
}
