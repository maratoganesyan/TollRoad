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
    }
}
