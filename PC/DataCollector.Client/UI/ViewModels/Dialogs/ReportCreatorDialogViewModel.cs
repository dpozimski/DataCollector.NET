using DataCollector.Client.DataAccess.Interfaces;
using DataCollector.Client.DataAccess.Models;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ViewModels.Chart;
using DataCollector.Client.UI.Views.Core;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using netoaster;
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
    /// Klasa implementująca obsługę konfiguracji nowego raportu.
    /// </summary>
    class ReportCreatorDialogViewModel : ViewModelBase
    {
        #region Constants
        private const int MaximumAllowedMeasures = 1000;
        #endregion

        #region Private Fields
        private IMeasureAccess measureAccess;
        private DateTime from = DateTime.Now.AddDays(-1),  to = DateTime.Now.AddHours(1);
        private ReportDefinitionViewModel reportDefinition;
        private ObservableCollection<MeasureDevice> measureDevices;
        private MeasureDevice selectedDevice;
        private Enum selectedMeasureType;
        #endregion

        #region Public Properties
        /// <summary>
        /// Liczba pobranych pomiarów.
        /// </summary>
        public int MeasuresCount =>
            reportDefinition?.ValuesCount ?? 0;
        /// <summary>
        /// Wybrany rodzaj pomiaru.
        /// </summary>
        public Enum SelectedMeasureType
        {
            get { return selectedMeasureType; }
            set { this.RaiseAndSetIfChanged(ref selectedMeasureType, value); }
        }
        /// <summary>
        /// Ograniczenie górnego pomiarów.
        /// </summary>
        public DateTime To
        {
            get { return to; }
            set { this.RaiseAndSetIfChanged(ref to, value); }
        }
        /// <summary>
        /// Ograniczenie dolne pomiarów.
        /// </summary>
        public DateTime From
        {
            get { return from; }
            set { this.RaiseAndSetIfChanged(ref from, value); }
        }
        /// <summary>
        /// Dostępne urządzenia pomiarowe.
        /// </summary>
        public ObservableCollection<MeasureDevice> MeasureDevices
        {
            get { return measureDevices; }
            set { this.RaiseAndSetIfChanged(ref measureDevices, value); }
        }
        /// <summary>
        /// Wybrane urządzenie pomiarowe.
        /// </summary>
        public MeasureDevice SelectedDevice
        {
            get { return selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref selectedDevice, value); }
        }
        /// <summary>
        /// Kolekcja danych wejściowych.
        /// </summary>
        public ReportDefinitionViewModel ReportDefinitiion
        {
            get { return reportDefinition; }
            set { this.RaiseAndSetIfChanged(ref reportDefinition, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda potwierdzenia konfigursacji raportu.
        /// </summary>
        public ReactiveCommand<object> ApplyReportCommand
        {
            get;private set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy ReportCreatorialogViewModel.
        /// </summary>
        public ReportCreatorDialogViewModel()
        {
            measureAccess = ServiceLocator.Resolve<IMeasureAccess>();
            InitCommands();
            InitData();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda pobierająca dane do kolekcji wejściowej.
        /// </summary>
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
        /// W zależności od preferencji zwraca kolekcję żądanych pomiarów.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DateTimePoint[]> GetMeasuresData()
        {
            IEnumerable<DateTimePoint[]> data = null;
            if (selectedMeasureType is MeasureType)
                data = measureAccess.GetMeasures((MeasureType)selectedMeasureType, selectedDevice, from, to);
            else
                data = measureAccess.GetMeasures((SphereMeasureType)selectedMeasureType, selectedDevice, from, to);
            return data.OrderBy(s=>s.First().DateTime);
        }
        /// <summary>
        /// Inicjalizacja komend użytkownika.
        /// </summary>
        private void InitCommands()
        {
            ApplyReportCommand = ReactiveCommand.Create();
            ApplyReportCommand.Subscribe(async s => await ApplyReportMethod());
        }
        /// <summary>
        /// Inicjalizacja danych bazodanowych.
        /// </summary>
        private void InitData()
        {
            //pobranie dostępnych urządzeń pomiarowych z historią pomiarów
            var measureDevices = measureAccess.GetMeasureDevices();
            MeasureDevices = new ObservableCollection<MeasureDevice>(measureDevices);
            SelectedDevice = MeasureDevices.FirstOrDefault();
            SelectedMeasureType = MeasureType.Temperature;
        }
        #endregion
    }
}
