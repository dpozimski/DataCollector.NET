using DataCollector.Client.UI.Converters;
using DataCollector.Client.UI.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// ViewModel obsługujący dane prezentacyjne kolekcji pomiarów.
    /// </summary>
    public class VariableVisualizationViewModel : ChartVisualizationViewModelBase
    {
        #region Constants
        private readonly long minuteTicks = TimeSpan.FromSeconds(60).Ticks;
        #endregion

        #region Private Fields
        private DateTime measureStartTime;
        private ObservableCollection<string> statisticLabels;
        private ChartValues<SeriesModel> statisticValues;
        private TimeSpan realTimeAnimationSpeed, measureDuration;
        private DispatcherTimer durationTimer;
        private SeriesModel averageValue, maxValue, minValue, actualValue;
        private long summaryAverageWeight;
        private QueueableChartValues<DateTimePoint> values;
        #endregion

        #region Public Properties
        /// <summary>
        /// Czas trwania pomiaru.
        /// </summary>
        public TimeSpan MeasureDuration
        {
            get { return measureDuration; }
            set { this.RaiseAndSetIfChanged(ref measureDuration, value); }
        }
        /// <summary>
        /// Czas rozpoczęcia pomiaru.
        /// </summary>
        public DateTime MeasureStartTime
        {
            get { return measureStartTime; }
            set { this.RaiseAndSetIfChanged(ref measureStartTime, value); }
        }
        /// <summary>
        /// Nagłówki wartości danych statystycznych.
        /// </summary>
        public ObservableCollection<string> StatisticLabels
        {
            get { return statisticLabels; }
            set { this.RaiseAndSetIfChanged(ref statisticLabels, value); }
        }
        /// <summary>
        /// Wartości danych statystycznych.
        /// </summary>
        public ChartValues<SeriesModel> StatisticValues
        {
            get { return statisticValues; }
            set { this.RaiseAndSetIfChanged(ref statisticValues, value); }
        }
        /// <summary>
        /// Czas trwania animacji wykresu w czasie rzeczywistym.
        /// </summary>
        public TimeSpan RealTimeChartAnimationSpeed
        {
            get { return realTimeAnimationSpeed; }
            set { this.RaiseAndSetIfChanged(ref realTimeAnimationSpeed, value); }
        }
        /// <summary>
        /// Wartości wejściowe.
        /// </summary>
        public QueueableChartValues<DateTimePoint> Values
        {
            get { return values; }
            set { this.RaiseAndSetIfChanged(ref values, value); }
        }
        #endregion

        #region Command
        /// <summary>
        /// Komenda resetu istniejących statystyk.
        /// </summary>
        public ReactiveCommand<object> ResetStatisticDataCommand
        {
            get; protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy SingleVariableVisualizationControl.
        /// </summary>
        public VariableVisualizationViewModel():base(string.Empty, string.Empty, (val) => string.Empty)
        {
            RealTimeChartAnimationSpeed = TimeSpan.FromMilliseconds(2000);

            durationTimer = new DispatcherTimer();
            durationTimer.Interval = RealTimeChartAnimationSpeed;

            ResetStatisticDataCommand = ReactiveCommand.Create();
            ResetStatisticDataCommand.Subscribe(s => ResetStatisticData());

            this.ObservableForProperty(s => s.Values).Subscribe(s => InitVisualizationData(s.Value));
            this.ObservableForProperty(s => s.Values, beforeChange: true).Subscribe(s => CleanSeriesResources(s.Value));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Czyszczenie subkrybowanych zdarzeń przed wpisaniem nowej wartości.
        /// </summary>
        /// <param name="data"></param>
        private void CleanSeriesResources(QueueableChartValues<DateTimePoint> data)
        {
            if (data != null)
            {
                data.PropertyChanged -= OnCollectionPropertyChanged;
                data.NoisyCollectionChanged -= OnNoisyCollectionChanged;
            }
        }
        /// <summary>
        /// Inicjalizacja danych wizualizacyjnych.
        /// </summary>
        /// <param name="data">nowa kolekcja wejściowa</param>
        private void InitVisualizationData(QueueableChartValues<DateTimePoint> data)
        {
            if (data != null)
            {
                var enumConverter = new EnumToStringDescription();
                YFormatter = (val) => $"{val} {data.Unit}";
                Header = enumConverter.ToDescription(data.Type);
                ResetStatisticData();
                OnEnabledStateChanged(data);
                data.PropertyChanged += new PropertyChangedEventHandler(OnCollectionPropertyChanged);
                data.NoisyCollectionChanged += new NoisyCollectionCollectionChanged<DateTimePoint>(OnNoisyCollectionChanged);
                durationTimer.Start();
                durationTimer.Tick += new EventHandler(OnDurationTimerCallback);
            }
            else
            {
                durationTimer.Stop();
                durationTimer.Tick -= OnDurationTimerCallback;
            }
        }
        /// <summary>
        /// Obsługa zdarzenia zegara, aktualizacja czasu trwania pomiaru/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDurationTimerCallback(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            MeasureDuration = (now - MeasureStartTime);
            MaxXAxis = now.Ticks;
            MinXAxis = now.Ticks - minuteTicks;
        }
        /// <summary>
        /// Obsługa zdarzenia zmiany właściwości aktywacji pomiarów.
        /// </summary>
        private void OnEnabledStateChanged(QueueableChartValues<DateTimePoint> collection)
        {
            durationTimer.IsEnabled = collection.IsEnabled;
            DisableAnimations = !collection.IsEnabled;
        }
        /// <summary>
        /// Reset danych statystycznych.
        /// </summary>
        private void ResetStatisticData()
        {
            averageValue = new SeriesModel("Średnia");
            maxValue = new SeriesModel("Maksimum");
            minValue = new SeriesModel("Minimum");
            actualValue = new SeriesModel("Bieżąca");

            StatisticValues = new ChartValues<SeriesModel>() { averageValue, maxValue, minValue, actualValue };
            StatisticLabels = new ObservableCollection<string>(statisticValues.Select(x => x.Name));

            summaryAverageWeight = 0;

            MeasureStartTime = DateTime.Now;
            MeasureDuration = TimeSpan.FromMilliseconds(0);
        }
        /// <summary>
        /// Obsługa zdarzenia zmiany w kolekcji wejściowej danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            QueueableChartValues<DateTimePoint> collection = sender as QueueableChartValues<DateTimePoint>;

            if (e.PropertyName.Equals(nameof(collection.IsEnabled)))
                OnEnabledStateChanged(collection);
        }
        /// <summary>
        /// Obsługa zdarzenia zmiany elementów w kolekcji wejściowej.
        /// </summary>
        /// <param name="oldItems"></param>
        /// <param name="newItems"></param>
        private void OnNoisyCollectionChanged(IEnumerable<DateTimePoint> oldItems, IEnumerable<DateTimePoint> newItems)
        {
            if (newItems != null)
            {
                var value = Math.Round((newItems.LastOrDefault()?.Value ?? 0.00), 4);

                var minValue = newItems.Min(s => s.Value);
                var maxValue = newItems.Max(s => s.Value);

                var newItemsCount = newItems.Count();
                var computedCount = summaryAverageWeight + newItemsCount;
                var computedAverage = ((summaryAverageWeight * this.averageValue.Value) + (newItemsCount * newItems.Average(s => s.Value))) / computedCount;
                this.summaryAverageWeight = computedCount;

                this.actualValue.Value = value;
                this.averageValue.Value = Math.Round(computedAverage, 4);

                this.minValue.Value = Math.Round((minValue > this.minValue.Value && this.minValue.IsTouched) ?
                                                 this.minValue.Value : minValue, 4);
                this.maxValue.Value = Math.Round((maxValue > this.maxValue.Value && this.minValue.IsTouched) ?
                                                 maxValue : this.maxValue.Value, 4);
            }
        }
        #endregion
    }
}
