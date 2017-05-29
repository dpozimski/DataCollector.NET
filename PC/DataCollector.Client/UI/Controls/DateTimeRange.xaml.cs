using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace DataCollector.Client.UI.Controls
{
    /// <summary>
    /// Interaction logic for DateTimeRange.xaml
    /// </summary>
    public partial class DateTimeRange : UserControl, INotifyPropertyChanged
    {
        #region Private Fields
        private string internalDate;
        private string internalTime;
        #endregion

        #region Publix XAML Static Properties
        /// <summary>
        /// Rejestracja właściwości wartości wejściowych.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime),
              typeof(DateTimeRange), new PropertyMetadata(DateTime.Now, OnSelectedDatePropertyChangedCallback));
        #endregion

        #region Public Properties
        /// <summary>
        /// Wybrany czas.
        /// </summary>
        public string InternalTime
        {
            get { return internalTime; }
            set
            {
                Set(ref internalTime, value);
                SetBindingDate();
            }
        }
        /// <summary>
        /// Wybrana data.
        /// </summary>
        public string InternalDate
        {
            get { return internalDate; }
            set
            {
                Set(ref internalDate, value);
                SetBindingDate();
            }
        }

        /// <summary>
        /// Wartości wejściowe.
        /// </summary>
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DateTimeRange.
        /// </summary>
        public DateTimeRange()
        {
            InitializeComponent();
            spMain.DataContext = this;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa zdarzenia nowego przypisania czasu.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedDatePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimeRange instance = d as DateTimeRange;
            DateTime time = (DateTime)e.NewValue;
            if(e.NewValue == null)
            {
                instance.internalTime = null;
                instance.internalDate = null;
            }
            else
            {
                instance.internalTime = (new DateTime() + time.TimeOfDay).ToShortTimeString();
                instance.internalDate = time.ToShortDateString();  
            }

            instance.PropertyChanged(instance, new PropertyChangedEventArgs(nameof(instance.InternalTime)));
            instance.PropertyChanged(instance, new PropertyChangedEventArgs(nameof(instance.InternalDate)));
        }
        /// <summary>
        /// Ustawienie daty wyjściowej.
        /// </summary>
        private void SetBindingDate()
        {
            try
            {
                DateTime time = DateTime.Parse(InternalTime);
                DateTime date = DateTime.Parse(InternalDate);

                SelectedDate = date + time.TimeOfDay;
            }
            catch(ArgumentNullException)
            {
                SelectedDate = DateTime.Now;
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
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
    }
}
