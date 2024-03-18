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
    /// Interaction logic for RoutsPage.xaml
    /// </summary>
    public partial class RoutsPage : Page
    {
        public RoutsPage()
        {
            InitializeComponent();
            RoutsDataGrid.ItemsSource = DbUtils.db.Routs.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoutsDataGrid.ItemsSource = DbUtils.db.Routs.ToList().Where(r => r.ToString().Contains(SearchTextBox.Text));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RoutsDataGrid.ItemsSource = DbUtils.db.Routs.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new RoutAddAndChange(false).ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var rout = (sender as Button).DataContext as Rout;
            new RoutAddAndChange(true, rout).ShowDialog();
        }
    }
}
