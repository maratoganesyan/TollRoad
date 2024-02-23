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
    /// Interaction logic for GenderPage.xaml
    /// </summary>
    public partial class GenderPage : Page
    {
        public GenderPage()
        {
            InitializeComponent();
            GenderDataGrid.ItemsSource = DbUtils.db.Genders.ToList();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GenderDataGrid.ItemsSource = DbUtils.db.Genders.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            GenderAddAndChange window = new GenderAddAndChange(false);
            window.ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Gender gender = (sender as Button).DataContext as Gender;
            if (gender != null)
            {
                GenderAddAndChange window = new GenderAddAndChange(true, gender);
                window.ShowDialog();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenderDataGrid.ItemsSource = DbUtils.db.Genders.ToList().Where(g => g.ToString().Contains(SearchTextBox.Text));
        }
    }
}
