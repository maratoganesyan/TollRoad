using ModernWpf.Controls;
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
using TollRoad.Windows.Pages;
using Wpf.Ui.Controls;

namespace TollRoad.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : UiWindow
    {
        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            EmployeeDataPanel.DataContext = employee;
            Global.CurrentEmployee = employee;
            MainContent.Content = new EmptyPage();
            SetAccess(employee);
        }

        private void SetAccess(Employee employee)
        {
            if(employee.IdRole == 1)
            {
                SprInf.Visibility = Visibility.Collapsed;
                CheckpointNVI.Visibility = Visibility.Collapsed;
                RoutNVI.Visibility = Visibility.Collapsed;
                EmployeeNVI.Visibility = Visibility.Collapsed;
            }
            if(employee.IdRole == 2)
            {
                SprInf.Visibility = Visibility.Collapsed;
                EmployeeNVI.Visibility = Visibility.Collapsed;
            }
        }

        

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;
            if (item.Tag is Type pageType && typeof(System.Windows.Controls.Page).IsAssignableFrom(pageType))
            {
                MainContent.Content = (System.Windows.Controls.Page)Activator.CreateInstance(pageType);
            }
            else if(item.Tag != null)
            {
                AuthWindow window = new AuthWindow();
                window.Show();
                this.Close();
            }
        }
    }
}
