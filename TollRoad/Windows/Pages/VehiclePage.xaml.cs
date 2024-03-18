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
    /// Interaction logic for VehiclePage.xaml
    /// </summary>
    public partial class VehiclePage : Page
    {
        public VehiclePage()
        {
            InitializeComponent();
            VehicleDataGrid.ItemsSource = DbUtils.db.Vehicles.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VehicleDataGrid.ItemsSource = DbUtils.db.Vehicles.ToList().Where(v => v.ToString().Contains(SearchTextBox.Text));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new VehicleAddAndChange(false).ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = (sender as Button).DataContext as Vehicle;
            new VehicleAddAndChange(true, vehicle).ShowDialog();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            VehicleDataGrid.ItemsSource = DbUtils.db.Vehicles.ToList();
        }
    }
}
