using DataCollector.Client.DataAccess.Interfaces;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.ViewModels.Chart;
using DataCollector.Client.UI.ViewModels.Dialogs;
using DataCollector.Client.UI.Views.Dialogs;
using LiveCharts;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels.Chart
{
    /// <summary>
    /// Klasa implementująca wyświetlanie danych archiwalnych.
    /// </summary>
    public class MeasurementsHistoryViewModel : RootViewModelBase
    {
        #region Private Fields
        private IMeasureAccess measureAccess;
        private ObservableCollection<ReportDefinitionViewModel> reportCollection;
        private ReportDefinitionViewModel selectedReport;
        #endregion

        #region Public Properties
        /// <summary>
        /// Aktualnie wybrany raport.
        /// </summary>
        public ReportDefinitionViewModel SelectedReport
        {
            get { return selectedReport; }
            set { this.RaiseAndSetIfChanged(ref selectedReport, value); }
        }
        /// <summary>
        /// Kolekcja zdefiniowanych raportów.
        /// </summary>
        public ObservableCollection<ReportDefinitionViewModel> ReportCollection
        {
            get { return reportCollection; }
            set { this.RaiseAndSetIfChanged(ref reportCollection, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda dodawania raportu.
        /// </summary>
        public ReactiveCommand<object> AddReportCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Komenda usuwania raportu.
        /// </summary>
        public ReactiveCommand<object> DeleteReportCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy MeasurementsHistoryViewModel.
        /// </summary>
        public MeasurementsHistoryViewModel()
        {
            measureAccess = ServiceLocator.Resolve<IMeasureAccess>();
            reportCollection = new ObservableCollection<ReportDefinitionViewModel>();
            InitCommands();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda inicjująca komendy użytkownika.
        /// </summary>
        private void InitCommands()
        {
            AddReportCommand = ReactiveCommand.Create();
            DeleteReportCommand = ReactiveCommand.Create();
            AddReportCommand.Subscribe(async s => await CreateReportMethod());
            DeleteReportCommand.Subscribe(async s => await DeleteReportMethod());
        }
        /// <summary>
        /// Metoda konfigurująca nowy raport wg użytkownika.
        /// </summary>
        private async Task CreateReportMethod()
        {
            ReportCreatorDialog creatorDialog = new ReportCreatorDialog()
            {
                DataContext = new ReportCreatorDialogViewModel()
            };
            var report = await DialogAccess.Show(creatorDialog, RootDialogId) as ReportDefinitionViewModel;
            if (report != null)
            {
                ReportCollection.Add(report);
                SelectedReport = ReportCollection.Last();
            }  
        }
        /// <summary>
        /// Usuwa aktualnie zaznaczony raport.
        /// </summary>
        private async Task DeleteReportMethod()
        {
            var builder = RequestModel.Create().Text("Czy na pewno chcesz usunąć wybrany raport?\r\n" +
                                    "Niezapisane dane zostaną utracone.");
            var result = await DialogAccess.ShowRequestAsync(builder);
            if(result == MessageDialogResult.Affirmative)
            {
                ReportCollection.Remove(SelectedReport);
                SelectedReport = ReportCollection.LastOrDefault();
            }
        }
        #endregion
    }
}
