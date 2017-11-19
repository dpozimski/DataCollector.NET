using DataCollector.Client.UI.ModulesAccess;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCollector.Client.UI.Models;
using System.Collections.ObjectModel;
using DataCollector.Client.UI.ViewModels.Core;
using LiveCharts.Defaults;
using System.ComponentModel;
using System.Windows;
using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// The ViewModel which implements the measures data adding in real-time.
    /// </summary>
    public class VisualizationViewModel : ViewModelBase
    {
        #region Private Fields
        private MeasureDeviceViewModel currentMeasureDevice;
        private VariableVisualizationViewModel selectedMeasure;
        private ObservableCollection<VariableVisualizationViewModel> measureCollection;
        private PropertyDescriptorCollection measureProperties;
        private IReadOnlyList<MeasureType> measureTypes;
        private ICommunicationServiceEventCallback communicationServiceCallback;
        #endregion

        #region Public Properties
        /// <summary>
        /// Current selected measure.
        /// </summary>
        public VariableVisualizationViewModel SelectedMeasure
        {
            get { return selectedMeasure; }
            set { this.RaiseAndSetIfChanged(ref selectedMeasure, value);
                OnSelectedMeasureIndexChanged(value);
            }
        }
        /// <summary>
        /// The measures values.
        /// </summary>
        public ObservableCollection<VariableVisualizationViewModel> MeasureCollection
        {
            get { return measureCollection; }
            set { this.RaiseAndSetIfChanged(ref measureCollection, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public VisualizationViewModel()
        {
            communicationServiceCallback = ServiceLocator.Resolve<ICommunicationServiceEventCallback>();
            measureTypes = ((MeasureType[])Enum.GetValues(typeof(MeasureType))).ToList();
            measureProperties = TypeDescriptor.GetProperties(typeof(Measures));
            MeasureCollection = new ObservableCollection<VariableVisualizationViewModel>();
            var observableForDeviceChanged = MainViewModel.ObservableForProperty(s => s.SelectedDevice);
            observableForDeviceChanged.Subscribe(device => OnCurrentDeviceChanged(device.Value));
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Called when [selected measure index changed].
        /// </summary>
        /// <param name="measure">The measure.</param>
        private void OnSelectedMeasureIndexChanged(VariableVisualizationViewModel measure)
        {
            if (MeasureCollection != null)
                for (int i = 0; i < MeasureCollection.Count; i++)
                    MeasureCollection[i].Values.IsEnabled = (measure == MeasureCollection[i]);
        }
        /// <summary>
        /// Called when [current device changed].
        /// </summary>
        /// <param name="measureDevice">The measure device.</param>
        private void OnCurrentDeviceChanged(MeasureDeviceViewModel measureDevice)
        {
            if(currentMeasureDevice?.MacAddress != measureDevice?.MacAddress)
            {
                communicationServiceCallback.MeasuresArrivedEvent -= OnMeasuresArrived;

                foreach (var item in MeasureCollection)
                    item.Values.Dispose();
                MeasureCollection.Clear();

                foreach(var item in (MeasureType[])Enum.GetValues(typeof(MeasureType)))
                {
                    var values = new QueueableChartValues<DateTimePoint>(item, EnumStrConverter.ToUnit(item));
                    MeasureCollection.Add(new VariableVisualizationViewModel() { Values = values });
                }

                communicationServiceCallback.MeasuresArrivedEvent += new EventHandler<MeasuresArrivedEventArgs>(OnMeasuresArrived);
                currentMeasureDevice = measureDevice;
                //the delay between adding a new measure
                Task.Factory.StartNew(() =>
                {
                    Task.Delay(100).Wait();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        SelectedMeasure = MeasureCollection.LastOrDefault()));
                });
            }
        }
        /// <summary>
        /// Called when [measures arrived].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MeasuresArrivedEventArgs"/> instance containing the event data.</param>
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
