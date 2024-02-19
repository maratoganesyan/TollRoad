using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;

namespace TollRoad.Windows
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : UiWindow
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        private async void LoadDb()
        {
            try
            {
                var employees = await Task.Run(() => DbUtils.db.Employees.ToList());
                new AuthWindow().Show();
                this.Close();
                
            }
            catch(Exception ex)
            {
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Ошибка", "Ошибка подключения к базе данных");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Transitions.ApplyTransition(this, TransitionType.FadeInWithSlide, 1000);
            LoadDb();
        }
    }
}
