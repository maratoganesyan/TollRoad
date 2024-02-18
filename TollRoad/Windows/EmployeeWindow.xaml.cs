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
        }

        

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;
            if (item.Tag is Type pageType && typeof(System.Windows.Controls.Page).IsAssignableFrom(pageType))
            {
                MainContent.Content = (System.Windows.Controls.Page)Activator.CreateInstance(pageType);
            }
        }
    }
}
