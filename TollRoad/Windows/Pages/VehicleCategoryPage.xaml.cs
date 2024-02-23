using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for VehicleCategoryPage.xaml
    /// </summary>
    public partial class VehicleCategoryPage : Page
    {
        public VehicleCategoryPage()
        {
            InitializeComponent();
            VehicleCategoryDataGrid.ItemsSource = DbUtils.db.VehicleCategories.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VehicleCategoryDataGrid.ItemsSource = DbUtils.db.VehicleCategories.ToList().Where(vc => vc.ToString().Contains(SearchTextBox.Text));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            VehicleCategoryDataGrid.ItemsSource = DbUtils.db.VehicleCategories.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new VehicleCategoryAddAndChange(false).ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button).DataContext as VehicleCategory;
            new VehicleCategoryAddAndChange(true, category).ShowDialog();
        }
    }
}
