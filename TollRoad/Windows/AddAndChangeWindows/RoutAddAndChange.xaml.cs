using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class RoutAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public RoutAddAndChange(bool changeMode, Rout rout = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            FirstCheckpointComboBox.ItemsSource = DbUtils.db.Checkpoints.ToList();
            SecondCheckpointComboBox.ItemsSource = DbUtils.db.Checkpoints.ToList();
            if(changeMode)
            {
                id = rout.Id;
                FirstCheckpointComboBox.SelectedItem = rout.IdFirstCheckpointNavigation;
                SecondCheckpointComboBox.SelectedItem = rout.IdSecondCheckpointNavigation;
                DistanceTextBox.Text = rout.DistanceInKm.ToString();
                FareTextBox.Text = rout.Fare.ToString();
            }
        }

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (FirstCheckpointComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Первый КПП не выбран");
                return false;
            }
            if (SecondCheckpointComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Второй КПП не выбран");
                return false;
            }
            if (FirstCheckpointComboBox.SelectedItem == SecondCheckpointComboBox.SelectedItem)
            {
                manager.Show("Ошибка сохранения", "Выбраны 2 одинаковые точки");
                return false;
            }
            if(DbUtils.db.Routs.Any(r => r.IdFirstCheckpointNavigation == FirstCheckpointComboBox.SelectedItem as Checkpoint &&
                                    r.IdSecondCheckpointNavigation == SecondCheckpointComboBox.SelectedItem as Checkpoint))
            {
                manager.Show("Ошибка сохранения", "Такой маршрут уже существует");
                return false;
            }
            if(!decimal.TryParse(DistanceTextBox.Text, out decimal _))
            {
                manager.Show("Ошибка сохранения", "Дистанция введена некорректно");
                return false;
            }
            if (!decimal.TryParse(FareTextBox.Text, out decimal _))
            {
                manager.Show("Ошибка сохранения", "Дистанция введена некорректно");
                return false;
            }
            return true;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    Rout rout;
                    if (_changeMode)
                        rout = DbUtils.db.Routs.FirstOrDefault(e => e.Id == id);
                    else
                        rout = new Rout();

                    rout.Fare = Math.Round(decimal.Parse(FareTextBox.Text), 2);
                    rout.DistanceInKm = Math.Round(decimal.Parse(DistanceTextBox.Text), 2);
                    rout.IdFirstCheckpoint = (FirstCheckpointComboBox.SelectedItem as Checkpoint).Id;
                    rout.IdSecondCheckpoint = (SecondCheckpointComboBox.SelectedItem as Checkpoint).Id;

                    if (!_changeMode)
                        DbUtils.db.Routs.Add(rout);

                    DbUtils.db.SaveChanges();
                    Close();

                    if(!_changeMode)
                    {
                        MessageBoxManager manager = new MessageBoxManager();
                        int number = DbUtils.db.Routs.OrderBy(e => e.Id).Last().Id;
                        manager.Show("Успешное сохранение", $"Маршрут сохранен под номером {number}");
                    }    

                }
                catch (Exception ex)
                {
                    MessageBoxManager manager = new MessageBoxManager();
                    manager.Show("Ошибка сохранения", "Ошибка сохранения данных в базу");
                }
            }
        }
    }
}
