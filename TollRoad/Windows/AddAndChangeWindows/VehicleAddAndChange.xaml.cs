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
    public partial class VehicleAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public VehicleAddAndChange(bool changeMode, Vehicle vehicle = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            CategoryComboBox.ItemsSource = DbUtils.db.VehicleCategories.ToList();
            if(changeMode)
            {
                id = vehicle.Id;
                StateNumberTextBox.Text = vehicle.StateNumber;
                WidthTextBox.Text = vehicle.WidthInMm.ToString();
                HeightTextBox.Text = vehicle.HeightInMm.ToString();
                WeightTextBox.Text = vehicle.WeightInKg.ToString();
                CategoryComboBox.SelectedItem = vehicle.CategoryOfVehicleNavigation;
            }
        }

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (DbUtils.db.Vehicles.Any(e => e.StateNumber == StateNumberTextBox.Text) && !_changeMode)
            {
                manager.Show("Ошибка сохранения", "Машина с таким гос. номером есть в базе");
                return false;
            }
            Regex regex = new Regex("/^[АВЕКМНОРСТУХ]\\d{3}(?<!000)[АВЕКМНОРСТУХ]{2}");
            if (regex.IsMatch(StateNumberTextBox.Text))
            {
                manager.Show("Ошибка сохранения", "Гос. номер введен неверно");
                return false;
            }
            if (!int.TryParse(HeightTextBox.Text, out int _))
            {
                manager.Show("Ошибка сохранения", "Высота ТС введена неверно");
                return false;
            }
            if (!int.TryParse(WidthTextBox.Text, out int _))
            {
                manager.Show("Ошибка сохранения", "Ширина ТС введена неверно");
                return false;
            }
            if (!int.TryParse(WeightTextBox.Text, out int _))
            {
                manager.Show("Ошибка сохранения", "Вес ТС введен неверно");
                return false;
            }
            if(CategoryComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Категория ТС не выбрана");
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
                    Vehicle vehicle;
                    if (_changeMode)
                        vehicle = DbUtils.db.Vehicles.FirstOrDefault(e => e.Id == id);
                    else
                        vehicle = new Vehicle();

                    vehicle.CategoryOfVehicle = (CategoryComboBox.SelectedItem as VehicleCategory).Id;
                    vehicle.StateNumber = StateNumberTextBox.Text;
                    vehicle.WidthInMm = int.Parse(WidthTextBox.Text);
                    vehicle.WeightInKg = int.Parse(WeightTextBox.Text);
                    vehicle.HeightInMm = int.Parse(HeightTextBox.Text);

                    if (!_changeMode)
                        DbUtils.db.Vehicles.Add(vehicle);

                    DbUtils.db.SaveChanges();
                    Close();

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
