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
    /// Klasa rozszerzająca funkcjonalność bazy ChartValues o warunkowe dodawanie danych.
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
        /// Jednostka pomiarowa.
        /// </summary>
        public string Unit
        {
            get { return unit; }
            set { Set(ref unit, value); }
        }
        /// <summary>
        /// Rodzaj pomiaru
        /// </summary>
        public MeasureType Type
        {
            get { return type; }
            set { Set(ref type, value); }
        }
        /// <summary>
        /// Flaga aktywacji kolekcji.
        /// W razie dezaktywacji dane są dodawane z trzykrotnie mniejszą częstotliwościa.
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
        /// Interwał pomiędzy dodaniem następnego elementu.
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
        /// Maksymalna liczba elementów w kolekcji.
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
        /// Konstruktor klasy QueueableChartValues.
        /// </summary>
        /// <param name="type">rodzaj pomiaru</param>
        public QueueableChartValues(MeasureType type, string unit)
        {
            this.Type = type;
            this.Unit = unit;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Próba dodania elementu do kolekcji.
        /// </summary>
        /// <param name="item">obiekt</param>
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
        /// Zwolnienie zasobów.
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
