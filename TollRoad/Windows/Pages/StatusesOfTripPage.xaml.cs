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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TollRoad.Models;
using TollRoad.Tools;

namespace TollRoad.Windows.Pages
{
    /// <summary>
    /// Interaction logic for StatusesOfTripPage.xaml
    /// </summary>
    public partial class StatusesOfTripPage : Page
    {
        public StatusesOfTripPage()
        {
            InitializeComponent();
            StatusesDataGrid.ItemsSource = DbUtils.db.StatusesOfTrips.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StatusesDataGrid.ItemsSource = DbUtils.db.StatusesOfTrips.ToList().Where(s => s.ToString().Contains(SearchTextBox.Text));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StatusesDataGrid.ItemsSource = DbUtils.db.StatusesOfTrips.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new StatusesOfTripAddAndChange(false).ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var status = (sender as Button).DataContext as StatusesOfTrip;
            new StatusesOfTripAddAndChange(true, status).ShowDialog();
        }
    }
}
