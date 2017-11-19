using DataCollector.Client.Translation;
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
    /// ViewModel which presents the visualization data of single variable.
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
        /// Gets or sets the duration of the measure.
        /// </summary>
        /// <value>
        /// The duration of the measure.
        /// </value>
        public TimeSpan MeasureDuration
        {
            get { return measureDuration; }
            set { this.RaiseAndSetIfChanged(ref measureDuration, value); }
        }
        /// <summary>
        /// Gets or sets the measure start time.
        /// </summary>
        /// <value>
        /// The measure start time.
        /// </value>
        public DateTime MeasureStartTime
        {
            get { return measureStartTime; }
            set { this.RaiseAndSetIfChanged(ref measureStartTime, value); }
        }
        /// <summary>
        /// Gets or sets the statistic labels.
        /// </summary>
        /// <value>
        /// The statistic labels.
        /// </value>
        public ObservableCollection<string> StatisticLabels
        {
            get { return statisticLabels; }
            set { this.RaiseAndSetIfChanged(ref statisticLabels, value); }
        }
        /// <summary>
        /// Gets or sets the statistic values.
        /// </summary>
        /// <value>
        /// The statistic values.
        /// </value>
        public ChartValues<SeriesModel> StatisticValues
        {
            get { return statisticValues; }
            set { this.RaiseAndSetIfChanged(ref statisticValues, value); }
        }
        /// <summary>
        /// Gets or sets the real time chart animation speed.
        /// </summary>
        /// <value>
        /// The real time chart animation speed.
        /// </value>
        public TimeSpan RealTimeChartAnimationSpeed
        {
            get { return realTimeAnimationSpeed; }
            set { this.RaiseAndSetIfChanged(ref realTimeAnimationSpeed, value); }
        }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public QueueableChartValues<DateTimePoint> Values
        {
            get { return values; }
            set { this.RaiseAndSetIfChanged(ref values, value); }
        }
        #endregion

        #region Command        
        /// <summary>
        /// Gets or sets the reset statistic data command.
        /// </summary>
        /// <value>
        /// The reset statistic data command.
        /// </value>
        public ReactiveCommand<object> ResetStatisticDataCommand
        {
            get; protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
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
        /// Cleans the series resources.
        /// </summary>
        /// <param name="data">The data.</param>
        private void CleanSeriesResources(QueueableChartValues<DateTimePoint> data)
        {
            if (data != null)
            {
                data.PropertyChanged -= OnCollectionPropertyChanged;
                data.NoisyCollectionChanged -= OnNoisyCollectionChanged;
            }
        }
        /// <summary>
        /// Initializes the visualization data.
        /// </summary>
        /// <param name="data">The data.</param>
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
        /// Called when [duration timer callback].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDurationTimerCallback(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            MeasureDuration = (now - MeasureStartTime);
            MaxXAxis = now.Ticks;
            MinXAxis = now.Ticks - minuteTicks;
        }
        /// <summary>
        /// Called when [enabled state changed].
        /// </summary>
        /// <param name="collection">The collection.</param>
        private void OnEnabledStateChanged(QueueableChartValues<DateTimePoint> collection)
        {
            durationTimer.IsEnabled = collection.IsEnabled;
            DisableAnimations = !collection.IsEnabled;
        }
        /// <summary>
        /// Resets the statistic data.
        /// </summary>
        private void ResetStatisticData()
        {
            averageValue = new SeriesModel(TranslationExtension.GetString("Average"));
            maxValue = new SeriesModel(TranslationExtension.GetString("Maximum"));
            minValue = new SeriesModel(TranslationExtension.GetString("Minimum"));
            actualValue = new SeriesModel(TranslationExtension.GetString("Current"));

            StatisticValues = new ChartValues<SeriesModel>() { averageValue, maxValue, minValue, actualValue };
            StatisticLabels = new ObservableCollection<string>(statisticValues.Select(x => x.Name));

            summaryAverageWeight = 0;

            MeasureStartTime = DateTime.Now;
            MeasureDuration = TimeSpan.FromMilliseconds(0);
        }
        /// <summary>
        /// Called when [collection property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            QueueableChartValues<DateTimePoint> collection = sender as QueueableChartValues<DateTimePoint>;

            if (e.PropertyName.Equals(nameof(collection.IsEnabled)))
                OnEnabledStateChanged(collection);
        }
        /// <summary>
        /// Called when [noisy collection changed].
        /// </summary>
        /// <param name="oldItems">The old items.</param>
        /// <param name="newItems">The new items.</param>
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
