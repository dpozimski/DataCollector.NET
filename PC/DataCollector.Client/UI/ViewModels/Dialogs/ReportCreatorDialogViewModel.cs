using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ViewModels.Chart;
using DataCollector.Client.UI.Views.Core;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using netoaster;
using netoaster.Enumes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCollector.Client.UI.ViewModels.Dialogs
{
    /// <summary>
    /// The creator dialog view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    class ReportCreatorDialogViewModel : ViewModelBase
    {
        #region Constants
        private const int MaximumAllowedMeasures = 1000;
        #endregion

        #region Private Fields
        private IMeasureAccessService measureAccess;
        private DateTime from = DateTime.Now.AddDays(-1),  to = DateTime.Now.AddHours(1);
        private ReportDefinitionViewModel reportDefinition;
        private ObservableCollection<MeasureDevice> measureDevices;
        private MeasureDevice selectedDevice;
        private Enum selectedMeasureType;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets the measures count.
        /// </summary>
        /// <value>
        /// The measures count.
        /// </value>
        public int MeasuresCount =>
            reportDefinition?.ValuesCount ?? 0;
        /// <summary>
        /// Gets or sets the type of the selected measure.
        /// </summary>
        /// <value>
        /// The type of the selected measure.
        /// </value>
        public Enum SelectedMeasureType
        {
            get { return selectedMeasureType; }
            set { this.RaiseAndSetIfChanged(ref selectedMeasureType, value); }
        }
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public DateTime To
        {
            get { return to; }
            set { this.RaiseAndSetIfChanged(ref to, value); }
        }
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public DateTime From
        {
            get { return from; }
            set { this.RaiseAndSetIfChanged(ref from, value); }
        }
        /// <summary>
        /// Gets or sets the measure devices.
        /// </summary>
        /// <value>
        /// The measure devices.
        /// </value>
        public ObservableCollection<MeasureDevice> MeasureDevices
        {
            get { return measureDevices; }
            set { this.RaiseAndSetIfChanged(ref measureDevices, value); }
        }
        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        /// <value>
        /// The selected device.
        /// </value>
        public MeasureDevice SelectedDevice
        {
            get { return selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref selectedDevice, value); }
        }
        /// <summary>
        /// Gets or sets the report definitiion.
        /// </summary>
        /// <value>
        /// The report definitiion.
        /// </value>
        public ReportDefinitionViewModel ReportDefinitiion
        {
            get { return reportDefinition; }
            set { this.RaiseAndSetIfChanged(ref reportDefinition, value); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets the apply report command.
        /// </summary>
        /// <value>
        /// The apply report command.
        /// </value>
        public ReactiveCommand<object> ApplyReportCommand
        {
            get;private set;
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCreatorDialogViewModel"/> class.
        /// </summary>
        public ReportCreatorDialogViewModel()
        {
            measureAccess = ServiceLocator.Resolve<IMeasureAccessService>();
            InitCommands();
            InitData();
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Applies the report method.
        /// </summary>
        /// <returns></returns>
        private async Task ApplyReportMethod()
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                try
                {
                    IEnumerable<DateTimePoint[]> data = GetMeasuresData();
                    int count = data.Count();

                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (count == 0)
                            DialogAccess.ShowToastNotification("Brak danych", ToastType.Info);
                        else if (count > MaximumAllowedMeasures)
                            DialogAccess.ShowToastNotification($"Moduł wizualizacji obsługuje maksymalnie {MaximumAllowedMeasures} pomiarów, a pobrano ich {count}\nNależy zmniejszyć zakres pomiarów.", ToastType.Info);
                        else
                        {
                            DialogAccess.ShowToastNotification($"Pobrano {count} pomiarów", ToastType.Success);
                            ReportDefinitiion = new ReportDefinitionViewModel(data, SelectedMeasureType);
                        }
                    }));
                }
                finally
                {
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.RaisePropertyChanged(nameof(MeasuresCount));
                        IsBusy = false;
                    }));
                }
            });
        }
        /// <summary>
        /// Gets the measures data.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DateTimePoint[]> GetMeasuresData()
        {
            IEnumerable<DateTimePoint[]> data = null;
            if (selectedMeasureType is MeasureType)
                data = measureAccess.GetMeasures((MeasureType)selectedMeasureType, selectedDevice, from, to);
            else
                data = measureAccess.GetSphereMeasures((SphereMeasureType)selectedMeasureType, selectedDevice, from, to);
            return data.OrderBy(s=>s.First().DateTime);
        }
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            ApplyReportCommand = ReactiveCommand.Create();
            ApplyReportCommand.Subscribe(async s => await ApplyReportMethod());
        }
        /// <summary>
        /// Initializes the data.
        /// </summary>
        private void InitData()
        {
            var measureDevices = measureAccess.GetMeasureDevices();
            MeasureDevices = new ObservableCollection<MeasureDevice>(measureDevices);
            SelectedDevice = MeasureDevices.FirstOrDefault();
            SelectedMeasureType = MeasureType.Temperature;
        }
        #endregion
    }
}
