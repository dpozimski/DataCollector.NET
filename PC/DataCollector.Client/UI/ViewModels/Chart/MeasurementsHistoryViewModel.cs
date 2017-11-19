using DataCollector.Client.Translation;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.Models;
using DataCollector.Client.UI.ModulesAccess;
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
    /// The ViewModel for displaying historical data of the measures.
    /// </summary>
    public class MeasurementsHistoryViewModel : RootViewModelBase
    {
        #region Private Fields
        private IMeasureAccessService measureAccess;
        private ObservableCollection<ReportDefinitionViewModel> reportCollection;
        private ReportDefinitionViewModel selectedReport;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets the selected report.
        /// </summary>
        /// <value>
        /// The selected report.
        /// </value>
        public ReportDefinitionViewModel SelectedReport
        {
            get { return selectedReport; }
            set { this.RaiseAndSetIfChanged(ref selectedReport, value); }
        }
        /// <summary>
        /// Gets or sets the report collection.
        /// </summary>
        /// <value>
        /// The report collection.
        /// </value>
        public ObservableCollection<ReportDefinitionViewModel> ReportCollection
        {
            get { return reportCollection; }
            set { this.RaiseAndSetIfChanged(ref reportCollection, value); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the add report command.
        /// </summary>
        /// <value>
        /// The add report command.
        /// </value>
        public ReactiveCommand<object> AddReportCommand
        {
            get;protected set;
        }
        /// <summary>
        /// Gets or sets the delete report command.
        /// </summary>
        /// <value>
        /// The delete report command.
        /// </value>
        public ReactiveCommand<object> DeleteReportCommand
        {
            get;protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public MeasurementsHistoryViewModel()
        {
            measureAccess = ServiceLocator.Resolve<IMeasureAccessService>();
            reportCollection = new ObservableCollection<ReportDefinitionViewModel>();
            InitCommands();
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            AddReportCommand = ReactiveCommand.Create();
            DeleteReportCommand = ReactiveCommand.Create();
            AddReportCommand.Subscribe(async s => await CreateReportMethod());
            DeleteReportCommand.Subscribe(async s => await DeleteReportMethod());
        }
        /// <summary>
        /// Creates the report method.
        /// </summary>
        /// <returns></returns>
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
        /// Deletes the report method.
        /// </summary>
        /// <returns></returns>
        private async Task DeleteReportMethod()
        {
            var builder = RequestModel.Create().Text(TranslationExtension.GetString("DoYouWantToDeleteCurrentReport"));
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
