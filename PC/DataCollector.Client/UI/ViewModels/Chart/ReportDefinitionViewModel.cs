using DataCollector.Client.DataAccess.Models;
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
    /// Klasa stanowiąca definicję nowego raportu.
    /// </summary>
    public class ReportDefinitionViewModel: ChartVisualizationViewModelBase
    {
        #region Static Fields
        private DataRange viewRange, dataRange;
        #endregion

        #region Constant
        /// <summary>
        /// Nomenklatura nazywania pomiarów na osi.
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
        /// Przesuń w lewo widok pomiarów.
        /// </summary>
        public ReactiveCommand<object> StepForwardCommand { get; protected set; }
        /// <summary>
        /// Przesuń w prawo widok pomiaró.
        /// </summary>
        public ReactiveCommand<object> StepBackwardCommand { get; protected set; }
        /// <summary>
        /// Zmniejszenie zakresu widoczności.
        /// </summary>
        public ReactiveCommand<object> ZoomInCommand { get; protected set; }
        /// <summary>
        /// Zwiększenie zakresu widoczności.
        /// </summary>
        public ReactiveCommand<object> ZoomOutCommand { get; protected set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Znacznik zakresu danych w widoku.
        /// </summary>
        public DataRange ViewRange
        {
            get { return viewRange; }
            set { this.RaiseAndSetIfChanged(ref viewRange, value); }
        }
        /// <summary>
        /// Znacznik zakresu wszystkich danych.
        /// </summary>
        public DataRange DataRange
        {
            get { return dataRange; }
            set { this.RaiseAndSetIfChanged(ref dataRange, value); }
        }
        /// <summary>
        /// Udostępnione wartości pomiarów.
        /// </summary>
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
        /// Konstruktor klasy ReportDefinitionModel.
        /// </summary>
        /// <param name="values">posortowane wartości</param>
        /// <param name="type">typ raportu</param>
        /// <param name="measureType">typ wartości</param>
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
        /// Inicjalizacja komend.
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
        /// Metoda obsługująca zmianę położenia danych w czasie.
        /// </summary>
        /// <param name="range">zakres przesuwający</param>
        /// <returns></returns>
        private void OnLocationPointRequest(TimeSpan range)
        {
            this.MaxXAxis += range.Ticks;
            this.MinXAxis += range.Ticks;
            UpdateView();
        }
        /// <summary>
        /// Metoda obsługująca zmianę objętości danych w widoku.
        /// </summary>
        /// <param name="range">zakres rozszerzający</param>
        /// <returns></returns>
        private void OnZoomRequest(TimeSpan range)
        {
            //równomiernie oddalenie/przybliżenie wykresu
            var halfRange = range.Ticks / 2;
            this.MaxXAxis += halfRange;
            this.MinXAxis -= halfRange;
            UpdateView();
        }
        /// <summary>
        /// Inicjalizacja widoku.
        /// <param name="inputValuesTypesCount">suma typów wartości wejsciowych</param>
        /// </summary>
        private void OnSourceValuesChanged(int inputValuesTypesCount)
        {
            view = new SeriesCollection();
            //inicjalizacja listy serii
            for (int i = 0; i < inputValuesTypesCount; i++)
                view.Add(new LineSeries() { Title = SeriesNameConvention[i], Values = new ChartValues<DateTimePoint>() });
        }
        /// <summary>
        /// Pobór danych do widoku.
        /// </summary>
        /// <returns></returns>
        private void UpdateView()
        {
            bool isViewRangeUpdated = false;
            //pobranie danych na podstawie min i max
            var data = values.Where(s =>
            {
                var dtPoint = s.Min(d=>d.DateTime);
                return dtPoint.Ticks > this.MinXAxis &&
                       dtPoint.Ticks < this.MaxXAxis;
            }).OrderBy(s=>s.Min(d=>d.DateTime)).ToList();
            //wyczyszczenie poprzednich wartości z widoku
            foreach (var item in view)
                item.Values.Clear();
            //wpisanie danych z odpowiedniej osi do odpowiadającej serii
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
