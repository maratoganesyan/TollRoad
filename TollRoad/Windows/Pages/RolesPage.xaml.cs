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
    /// Interaction logic for RolesPage.xaml
    /// </summary>
    public partial class RolesPage : Page
    {
        public RolesPage()
        {
            InitializeComponent();
            RoleDataGrid.ItemsSource = DbUtils.db.Roles.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoleDataGrid.ItemsSource = DbUtils.db.Roles.ToList().Where(r => r.ToString().Contains(SearchTextBox.Text)).ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RoleAddAndChange window = new RoleAddAndChange(false);
            window.ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var role = (sender as Button).DataContext as Role;
            var window = new RoleAddAndChange(true, role);
            window.ShowDialog();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RoleDataGrid.ItemsSource = DbUtils.db.Roles.ToList();
        }
    }
}
