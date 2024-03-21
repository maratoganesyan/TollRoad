using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TollRoad.Models;
using TollRoad.Tools;
using TollRoad.Tools.ReportModels;
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;

namespace TollRoad.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class CheckpointReportWindow : UiWindow
    {
        Checkpoint checkpoint;
        public CheckpointReportWindow(Checkpoint _checkpoint)
        {
            InitializeComponent();
            checkpoint = _checkpoint;
            StartDatePicker.SelectedDate = DbUtils.db.Trips.Min(t => t.StartDateTimeOfTrip);
            EndDatePicker.SelectedDate = DbUtils.db.Trips.Max(t => t.EndDateTimeOfTrip);
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxManager manager = new MessageBoxManager();
            if(EndDatePicker.SelectedDate.Value < StartDatePicker.SelectedDate.Value)
            {
                manager.Show("Ошибка генерации", "Начальная дата должна быть раньше конечной");
                return;
            }
            try
            {
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;
                List<Trip> trips = DbUtils.db.Trips.Where(r => (r.IdRouteNavigation.IdFirstCheckpoint == checkpoint.Id
                                                          || r.IdRouteNavigation.IdSecondCheckpoint == checkpoint.Id)
                                                          && startDate <= r.StartDateTimeOfTrip && endDate >= r.EndDateTimeOfTrip).ToList();
                if(trips.Count() == 0)
                {
                    manager.Show("Ошибка генерации", "Нет поездок в этот промежуток времени");
                    return;
                }
                CheckpointReportModel model = new CheckpointReportModel(checkpoint, trips, startDate, endDate);
                Load.Visibility = Visibility.Visible;
                GenerateButton.IsEnabled = false;
                await ReportGeneration.DoACheckpointReportAsync(model);
                GenerateButton.IsEnabled = true;
                Load.Visibility = Visibility.Collapsed;

            }
            catch(Exception ex)
            {
                manager.Show("Ошибка генерации", "Ошибка генерации отчёта");
            }
        }
    }
}
