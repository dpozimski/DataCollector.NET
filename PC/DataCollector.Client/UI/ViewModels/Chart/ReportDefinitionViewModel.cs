using DataCollector.Client.UI.Converters;
using DataCollector.Client.UI.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// Class which represents a single report.
    /// </summary>
    public class ReportDefinitionViewModel: ChartVisualizationViewModelBase
    {
        #region Private Fields
        private DataRange viewRange, dataRange;
        #endregion

        #region Constant
        /// <summary>
        /// The names of the axis.
        /// </summary>
        private readonly string[] SeriesNameConvention = new string[] { "X", "Y", "Z" };
        #endregion

        #region Private Fields
        private readonly IEnumerable<DateTimePoint[]> values;
        private SeriesCollection view;
        private TimeSpan switchRange = TimeSpan.FromSeconds(30);
        private TimeSpan initialDataRange = TimeSpan.FromSeconds(150);
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the step forward command.
        /// </summary>
        /// <value>
        /// The step forward command.
        /// </value>
        public ReactiveCommand<object> StepForwardCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the step backward command.
        /// </summary>
        /// <value>
        /// The step backward command.
        /// </value>
        public ReactiveCommand<object> StepBackwardCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the zoom in command.
        /// </summary>
        /// <value>
        /// The zoom in command.
        /// </value>
        public ReactiveCommand<object> ZoomInCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the zoom out command.
        /// </summary>
        /// <value>
        /// The zoom out command.
        /// </value>
        public ReactiveCommand<object> ZoomOutCommand { get; protected set; }
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets the view range.
        /// </summary>
        /// <value>
        /// The view range.
        /// </value>
        public DataRange ViewRange
        {
            get { return viewRange; }
            set { this.RaiseAndSetIfChanged(ref viewRange, value); }
        }
        /// <summary>
        /// Gets or sets the data range.
        /// </summary>
        /// <value>
        /// The data range.
        /// </value>
        public DataRange DataRange
        {
            get { return dataRange; }
            set { this.RaiseAndSetIfChanged(ref dataRange, value); }
        }
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public SeriesCollection View
        {
            get { return view; }
            set { this.RaiseAndSetIfChanged(ref view, value); }
        }
        /// <summary>
        /// Liczba punktów pomiarowych wejściowych.
        /// </summary>
        public int ValuesCount => values.Count();
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDefinitionViewModel"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="measureType">Type of the measure.</param>
        /// <CreatedOn>19.11.2017 14:24</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public ReportDefinitionViewModel(IEnumerable<DateTimePoint[]> values, Enum measureType)
           :base(EnumStrConverter.ToDescription(measureType), EnumStrConverter.ToUnit(measureType), (val) => $"{new DateTime((long)Math.Abs(val))}")
        {
            this.values = values;

            this.ViewRange = new DataRange();
            this.DataRange = new DataRange();

            this.DataRange.Update(values.First().Min(s => s.DateTime), values.Last().Max(s => s.DateTime));

            this.initialDataRange = TimeSpan.FromTicks((20 * (dataRange.LastStamp.Ticks - dataRange.FirstStamp.Ticks)) / 100);
            this.switchRange = TimeSpan.FromTicks(this.initialDataRange.Ticks / 2);

            this.MaxXAxis = dataRange.LastStamp.Ticks;
            long defaultDiff = MaxXAxis - initialDataRange.Ticks;
            this.MinXAxis = (defaultDiff < dataRange.FirstStamp.Ticks) ? dataRange.FirstStamp.Ticks : defaultDiff;

            OnSourceValuesChanged(values.First().Count());

            InitCommands();

            OnLocationPointRequest(TimeSpan.FromSeconds(0));
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            StepBackwardCommand = ReactiveCommand.Create(Observable.CombineLatest(
                ViewRange.WhenAnyValue(d=>d.FirstStamp), DataRange.WhenAnyValue(d=>d.FirstStamp),
                                    (view, data) => (view - switchRange) >= data));
            StepForwardCommand = ReactiveCommand.Create(Observable.CombineLatest(
                ViewRange.WhenAnyValue(d=>d.LastStamp), DataRange.WhenAnyValue(d=>d.LastStamp),
                                    (view, data) => (view + switchRange) <= data));
            ZoomInCommand = ReactiveCommand.Create(Observable.CombineLatest(
                ViewRange.WhenAnyValue(d=>d.LastStamp), ViewRange.WhenAnyValue(d=>d.FirstStamp),
                                    (last, first) => (last - switchRange) > (first + switchRange)));
            ZoomOutCommand = ReactiveCommand.Create(Observable.CombineLatest(
                ViewRange.WhenAnyValue(d=>d.FirstStamp), DataRange.WhenAnyValue(d=>d.FirstStamp), ViewRange.WhenAnyValue(d=>d.LastStamp), DataRange.WhenAnyValue(d=>d.LastStamp),
                                    (firstView, firstData, lastView, lastData) => (firstView - switchRange) >= firstData &&
                                                                                    (lastView + switchRange) <= lastData));

            StepBackwardCommand.Subscribe(s => OnLocationPointRequest(-switchRange));
            StepForwardCommand.Subscribe(s => OnLocationPointRequest(switchRange));
            ZoomInCommand.Subscribe(s => OnZoomRequest(-switchRange));
            ZoomOutCommand.Subscribe(s => OnZoomRequest(switchRange));
        }

        /// <summary>
        /// Called when [location point request].
        /// </summary>
        /// <param name="range">The range.</param>
        private void OnLocationPointRequest(TimeSpan range)
        {
            this.MaxXAxis += range.Ticks;
            this.MinXAxis += range.Ticks;
            UpdateView();
        }
        /// <summary>
        /// Called when [zoom request].
        /// </summary>
        /// <param name="range">The range.</param>
        private void OnZoomRequest(TimeSpan range)
        {
            var halfRange = range.Ticks / 2;
            this.MaxXAxis += halfRange;
            this.MinXAxis -= halfRange;
            UpdateView();
        }
        /// <summary>
        /// Called when [source values changed].
        /// </summary>
        /// <param name="inputValuesTypesCount">The input values types count.</param>
        private void OnSourceValuesChanged(int inputValuesTypesCount)
        {
            view = new SeriesCollection();
            //inicjalizacja listy serii
            for (int i = 0; i < inputValuesTypesCount; i++)
                view.Add(new LineSeries() { Title = SeriesNameConvention[i], Values = new ChartValues<DateTimePoint>() });
        }
        /// <summary>
        /// Updates the view.
        /// </summary>
        private void UpdateView()
        {
            bool isViewRangeUpdated = false;
            //gets the data depends of min and max range
            var data = values.Where(s =>
            {
                var dtPoint = s.Min(d=>d.DateTime);
                return dtPoint.Ticks > this.MinXAxis &&
                       dtPoint.Ticks < this.MaxXAxis;
            }).OrderBy(s=>s.Min(d=>d.DateTime)).ToList();
            //clears the previous values
            foreach (var item in view)
                item.Values.Clear();
            //move the data to series
            for (int i = 0; i < view.Count; i++)
            {
                IEnumerable<DateTimePoint> singleSeriesData = data.Select(s => s[i]);
                if(!isViewRangeUpdated && singleSeriesData.Count() > 0)
                {
                    ViewRange.Update(singleSeriesData.First().DateTime, singleSeriesData.Last().DateTime);
                    isViewRangeUpdated = true;
                }
                view[i].Values.AddRange(singleSeriesData);
            }
        }
        #endregion
    }
}
