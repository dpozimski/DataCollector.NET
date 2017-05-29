using netoaster.Enumes;
using System;
using System.Windows;
using System.Windows.Threading;

namespace netoaster
{
    public partial class ToasterWindow
    {
        public ToasterWindow(Window owner)
        {
            InitializeComponent();

            var story = ToastSupport.GetAnimation(Notification);
            story.Completed += (sender, args) => { Close(); };
            story.Begin(Notification);

            Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                var topLeftDict = ToastSupport.GetTopandLeft(this, Margins);
                Top = topLeftDict["Top"];
                Left = topLeftDict["Left"];
            }));

            if (owner != null)
                owner.Unloaded += Owner_Unloaded;
        }
        public static readonly DependencyProperty ToasterTitleProperty = DependencyProperty.Register("Title",
          typeof(string), typeof(ToasterWindow), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message",
            typeof(string), typeof(ToasterWindow), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MarginsProperty = DependencyProperty.Register("Margins",
            typeof(double), typeof(ToasterWindow), new PropertyMetadata(0D));


        public string ToasterTitle
        {
            get { return (string)GetValue(ToasterTitleProperty); }
            set { SetValue(ToasterTitleProperty, value); }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public double Margins
        {
            get { return (double)GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        protected void Owner_Unloaded(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
