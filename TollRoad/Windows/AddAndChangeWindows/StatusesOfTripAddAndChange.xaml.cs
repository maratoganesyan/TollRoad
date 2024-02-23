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
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;

namespace TollRoad.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class StatusesOfTripAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public StatusesOfTripAddAndChange(bool changeMode, StatusesOfTrip status = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            if(changeMode)
            {
                id = status.Id;
                StatusNameTextBox.Text = status.StatusName;
            }
        }

        private bool Validation()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (StatusNameTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения","Поля не заполнены");
                return false;
            }
            if (DbUtils.db.StatusesOfTrips.ToList().Any(ce => ce.StatusName == StatusNameTextBox.Text && ce.Id != id))
            {
                manager.Show("Ошибка сохранения", "Такая запись уже есть в базе");
                return false;
            }
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validation())
                    return;
                StatusesOfTrip status;
                if (_changeMode)
                    status = DbUtils.db.StatusesOfTrips.FirstOrDefault(c => c.Id == id);
                else
                    status = new StatusesOfTrip();

                status.StatusName = StatusNameTextBox.Text;

                if (!_changeMode)
                    DbUtils.db.StatusesOfTrips.Add(status);

                DbUtils.db.SaveChanges();
                Close();

            }
            catch (Exception ex)
            {
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Ошибка сохранения", "Ошибка подключения к базе данных");
            }
        }
    }
}
