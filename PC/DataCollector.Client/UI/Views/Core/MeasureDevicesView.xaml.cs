using DataCollector.Client.UI.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataCollector.Client.UI.Views.Core
{
    /// <summary>
    /// Interaction logic for MeasureDevices.xaml
    /// </summary>
    public partial class MeasureDevicesView : UserControl
    {
        #region Publix XAML Static Properties
        /// <summary>
        /// Rejestracja właściwości aktualnie wybranego urządzenia.
        /// </summary>
        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof(MeasureDeviceViewModel),
              typeof(MeasureDevicesView), new PropertyMetadata(null));
        #endregion

        #region Public Properties
        /// <summary>
        /// Aktualnie wybrane urządzenie.
        /// </summary>
        public MeasureDeviceViewModel SelectedDevice
        {
            get { return (MeasureDeviceViewModel)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy MeasureDevicesView.
        /// </summary>
        public MeasureDevicesView()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// Obsługa zdarzenia wpisania klawisza dla pola numerycznego.
        /// Wpisywanie tylko wartości całkowitych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
