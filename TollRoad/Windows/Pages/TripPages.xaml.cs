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
    /// Interaction logic for TripPages.xaml
    /// </summary>
    public partial class TripPages : Page
    {
        public TripPages()
        {
            InitializeComponent();
            TripDataGrid.ItemsSource = DbUtils.db.Trips.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TripDataGrid.ItemsSource = DbUtils.db.Trips.ToList().Where(t => t.ToString().Contains(SearchTextBox.Text));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TripDataGrid.ItemsSource = DbUtils.db.Trips.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new TripAddAndChange(false).ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var trip = (sender as Button).DataContext as Trip;
            new TripAddAndChange(true, trip).ShowDialog();
        }
    }
}
