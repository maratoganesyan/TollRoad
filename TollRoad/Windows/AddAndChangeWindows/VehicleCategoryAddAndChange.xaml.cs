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
    public partial class VehicleCategoryAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public VehicleCategoryAddAndChange(bool changeMode, VehicleCategory category = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            if(changeMode)
            {
                id = category.Id;
                CategoryNameTextBox.Text = category.CategoryName;
                FaceCoefficientTextBox.Text = category.FaceCoefficient.ToString();
            }
        }

        private bool Validation()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (CategoryNameTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения","Поля не заполнены");
                return false;
            }
            if(!decimal.TryParse(FaceCoefficientTextBox.Text, out decimal _))
            {
                manager.Show("Ошибка сохранения", "Коэффициент записан не в виде числа");
                return false;
            }
            if (DbUtils.db.StatusesOfTrips.ToList().Any(ce => ce.StatusName == CategoryNameTextBox.Text && ce.Id != id))
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
                VehicleCategory category;
                if (_changeMode)
                    category = DbUtils.db.VehicleCategories.FirstOrDefault(c => c.Id == id);
                else
                    category = new VehicleCategory();

                category.CategoryName = CategoryNameTextBox.Text;
                category.FaceCoefficient = decimal.Parse(FaceCoefficientTextBox.Text);

                if (!_changeMode)
                    DbUtils.db.VehicleCategories.Add(category);

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
