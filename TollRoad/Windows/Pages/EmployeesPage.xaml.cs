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
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            UsersOutput.ItemsSource = DbUtils.db.Employees.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeAddAndChange(false).ShowDialog();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UsersOutput.ItemsSource = DbUtils.db.Employees.ToList();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var employee = (sender as Button).DataContext as Employee;
            new EmployeeAddAndChange(true, employee).ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersOutput.ItemsSource = DbUtils.db.Employees.ToList().Where(u => u.ToString().Contains(SearchTextBox.Text));
        }
    }
}
