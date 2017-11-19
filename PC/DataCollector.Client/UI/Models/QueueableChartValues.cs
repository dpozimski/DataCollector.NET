using DataCollector.Client.UI.DataAccess;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Class which extend the default behavior of the
    /// <see cref="ChartValues<T>"/> with conditional adding of items.
    /// </summary>
    public class QueueableChartValues<T> : ChartValues<T>, INotifyPropertyChanged, IDisposable
    {
        #region Private Fields
        private DateTime lastTimeStamp;
        private bool isEnabled;
        private TimeSpan intervalBetweenItemInserting = TimeSpan.FromMilliseconds(2000);
        private string unit;
        private MeasureType type;
        private int countLimit = 40;
        private int decreaseMultiplierAfterDisable = 3;
        #endregion

        #region Public Properties
        /// <summary>
        /// The measure unit.
        /// </summary>
        public string Unit
        {
            get { return unit; }
            set { Set(ref unit, value); }
        }
        /// <summary>
        /// The measure type.
        /// </summary>
        public MeasureType Type
        {
            get { return type; }
            set { Set(ref type, value); }
        }
        /// <summary>
        /// If the value is false the data will be added 3 times less often than usual.
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// The interval between adding a new elements.
        /// </summary>
        public TimeSpan IntervalBetweenItemInserting
        {
            get
            {
                return IsEnabled ? intervalBetweenItemInserting :
                                   new TimeSpan(intervalBetweenItemInserting.Ticks * decreaseMultiplierAfterDisable);
            }
            set
            {
                intervalBetweenItemInserting = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// The maximum count of the items.
        /// </summary>
        public int CountLimit
        {
            get
            {
                return IsEnabled ? countLimit :
                                   (countLimit / decreaseMultiplierAfterDisable);
            }
            set
            {
                countLimit = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="type">the measure type</param>
        /// <param name="unit">the measure unit</param>
        public QueueableChartValues(MeasureType type, string unit)
        {
            this.Type = type;
            this.Unit = unit;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Tries to add a new element to collection.
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>sukces</returns>
        public bool TryAdd(T item)
        {
            bool success = false;

            var dateTime = DateTime.Now;
            if (dateTime >= lastTimeStamp.Add(IntervalBetweenItemInserting))
            {
                if (Count > CountLimit)
                    Application.Current.Dispatcher.Invoke(new Action(() => RemoveAt(0)));
                Application.Current.Dispatcher.Invoke(new Action(() => Add(item)));

                lastTimeStamp = dateTime;
                success = true;
            }

            return success;
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<TProperty>(ref TProperty storage, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region IDisposable
        /// <summary>
        /// Releases the manages resources taken by this instance.
        /// </summary>
        public void Dispose()
        {
            IsEnabled = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                Clear()));
        }
        #endregion
    }
}
