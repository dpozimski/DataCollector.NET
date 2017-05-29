using DataCollector.Client.Communication.Interfaces;
using DataCollector.Client.DataAccess.Models;
using DataCollector.Client.UI.ModulesAccess;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCollector.Client.Communication.Models;
using DataCollector.Client.UI.Views.Core;
using DataCollector.Client.UI.Models;
using System.Collections.ObjectModel;
using DataCollector.Client.UI.ViewModels.Core;
using LiveCharts.Defaults;
using System.Diagnostics;
using DataCollector.Client.UI.ViewModels.Chart;
using System.ComponentModel;
using DataCollector.Device.Models;
using System.Windows;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// Klasa implementujący obsługę danych wizualizacyjnych w czasie rzeczywistym.
    /// </summary>
    public class VisualizationViewModel : ViewModelBase
    {
        #region Private Fields
        private MeasureDeviceViewModel currentMeasureDevice;
        private VariableVisualizationViewModel selectedMeasure;
        private ObservableCollection<VariableVisualizationViewModel> measureCollection;
        private ICommunication webCommunication;
        private PropertyDescriptorCollection measureProperties;
        private IReadOnlyList<MeasureType> measureTypes;
        #endregion

        #region Public Properties
        /// <summary>
        /// Aktualnie wybrany pomiar.
        /// </summary>
        public VariableVisualizationViewModel SelectedMeasure
        {
            get { return selectedMeasure; }
            set { this.RaiseAndSetIfChanged(ref selectedMeasure, value);
                OnSelectedMeasureIndexChanged(value);
            }
        }
        /// <summary>
        /// Wartości pomiarowe.
        /// </summary>
        public ObservableCollection<VariableVisualizationViewModel> MeasureCollection
        {
            get { return measureCollection; }
            set { this.RaiseAndSetIfChanged(ref measureCollection, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy VisualizationViewModel.
        /// </summary>
        public VisualizationViewModel()
        {
            measureTypes = ((MeasureType[])Enum.GetValues(typeof(MeasureType))).ToList();
            measureProperties = TypeDescriptor.GetProperties(typeof(Measures));
            MeasureCollection = new ObservableCollection<VariableVisualizationViewModel>();
            webCommunication = ServiceLocator.Resolve<ICommunication>();
            var observableForDeviceChanged = MainViewModel.ObservableForProperty(s => s.SelectedDevice);
            observableForDeviceChanged.Subscribe(device => OnCurrentDeviceChanged(device.Value));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa zdarzenia zmiany aktualnie wybranego elementu.
        /// Dezaktywacja kolekcji nie będących na pierwszym planie.
        /// <param name="index">wybrany indeks</param>
        /// </summary>
        private void OnSelectedMeasureIndexChanged(VariableVisualizationViewModel measure)
        {
            if (MeasureCollection != null)
                for (int i = 0; i < MeasureCollection.Count; i++)
                    MeasureCollection[i].Values.IsEnabled = (measure == MeasureCollection[i]);
        }
        /// <summary>
        /// Obsługa zmiany aktualnie obsługiwanego urządzenia.
        /// </summary>
        private void OnCurrentDeviceChanged(MeasureDeviceViewModel measureDevice)
        {
            if(currentMeasureDevice?.MacAddress != measureDevice?.MacAddress)
            {
                webCommunication.MeasuresArrived -= OnMeasuresArrived;

                foreach (var item in MeasureCollection)
                    item.Values.Dispose();
                MeasureCollection.Clear();

                foreach(var item in (MeasureType[])Enum.GetValues(typeof(MeasureType)))
                {
                    var values = new QueueableChartValues<DateTimePoint>(item, EnumStrConverter.ToUnit(item));
                    MeasureCollection.Add(new VariableVisualizationViewModel() { Values = values });
                }

                webCommunication.MeasuresArrived += new EventHandler<MeasuresArrivedEventArgs>(OnMeasuresArrived);
                currentMeasureDevice = measureDevice;
                //opóźnienie oznaczenia nowego pomiaru
                Task.Factory.StartNew(() =>
                {
                    Task.Delay(100).Wait();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SelectedMeasure = MeasureCollection.LastOrDefault()));
                });
            }
        }
        /// <summary>
        /// Obsługa zdarzenia nadejścia pomiarów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMeasuresArrived(object sender, MeasuresArrivedEventArgs e)
        {
            if (currentMeasureDevice?.MacAddress != e.Source.MacAddress)
                return;

            foreach (PropertyDescriptor prop in measureProperties)
            {
                if (prop.PropertyType == typeof(float?))
                {
                    var type = measureTypes.Single(s => s.ToString() == prop.Name);
                    var chartModel = new DateTimePoint(e.TimeStamp, (float?)prop.GetValue(e.Value) ?? 0);
                    MeasureCollection.Single(s => s.Values.Type == type).Values.TryAdd(chartModel);
                }
            }
        }
        #endregion
    }
}
