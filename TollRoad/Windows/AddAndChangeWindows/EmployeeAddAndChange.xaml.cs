using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class EmployeeAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;

        string photoPath;
        public EmployeeAddAndChange(bool changeMode, Employee employee = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            InitComboBoxes();
            if (changeMode)
                InitEmployee(employee);
        }

        private void InitComboBoxes()
        {
            RolesComboBox.ItemsSource = DbUtils.db.Roles.ToList();
            GenderComboBox.ItemsSource = DbUtils.db.Genders.ToList();
            CheckpointsComboBox.ItemsSource = DbUtils.db.Checkpoints.ToList();
        }

        private void InitEmployee(Employee employee)
        {
            id = employee.Id;
            SurnameTextBox.Text = employee.Surname;
            NameTextBox.Text = employee.Name;
            PatronymicTextBox.Text = employee.Patronymic;
            LoginTextBox.Text = employee.Login;
            PasswordTextBox.Text = employee.Password;
            PhoneNumberTextBox.Text = employee.PhoneNumber;
            RolesComboBox.SelectedItem = employee.IdRoleNavigation;
            GenderComboBox.SelectedItem = employee.IdGenderNavigation;
            CheckpointsComboBox.SelectedItem = employee.IdCheckpointNavigation;
            UserPhoto.Source = new ImageSourceConverter().ConvertFrom(employee.Photo) as ImageSource;
        }

        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                photoPath = openFileDialog.FileName;
                UserPhoto.Source = new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(photoPath)) as ImageSource;
            }
        }

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (DbUtils.db.Employees.Any(e => e.Login == LoginTextBox.Text) && !_changeMode)
            {
                manager.Show("Ошибка сохранения", "Пользователь с таким логином есть в системе, введите другой логин");
                return false;
            }
            if (SurnameTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения", "Фамилия не заполнена");
                return false;
            }
            if (NameTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения", "Имя не заполнено");
                return false;
            }
            if (PatronymicTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения", "Отчество не заполнено");
                return false;
            }
            if(PhoneNumberTextBox.Text.Contains('_'))
            {
                manager.Show("Ошибка сохранения", "Номер телефона не заполнен");
                return false;
            }
            if (LoginTextBox.Text.Length < 8)
            {
                manager.Show("Ошибка сохранения", "Логин должен содержать минимум 8");
                return false;
            }
            if (PasswordTextBox.Text.Length < 8)
            {
                manager.Show("Ошибка сохранения", "Пароль должен содержать минимум 8");
                return false;
            }
            if(RolesComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Роль не выбрана");
                return false;
            }
            if(CheckpointsComboBox.SelectedItem == null && RolesComboBox.SelectedItem is Role role && role.Id == 1)
            {
                manager.Show("Ошибка сохранения", "КПП не выбрано");
                return false;
            }
            if (CheckpointsComboBox.SelectedItem != null && RolesComboBox.SelectedItem is Role role2 && role2.Id != 1)
            {
                manager.Show("Ошибка сохранения", "Директор или администратор не работают на КПП");
                return false;
            }
            if (GenderComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Пол не выбран");
                return false;
            }
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                try
                {
                    Employee employee;
                    if (_changeMode)
                        employee = DbUtils.db.Employees.FirstOrDefault(e => e.Id == id);
                    else
                        employee = new Employee();

                    employee.Surname = SurnameTextBox.Text;
                    employee.Name = NameTextBox.Text;
                    employee.Patronymic = PatronymicTextBox.Text.Length == 0 ? null : PatronymicTextBox.Text;
                    employee.PhoneNumber = PhoneNumberTextBox.Text;
                    employee.Login = LoginTextBox.Text;
                    employee.Password = PasswordTextBox.Text;
                    employee.IdRole = (RolesComboBox.SelectedItem as Role).Id;
                    employee.IdGender = (GenderComboBox.SelectedItem as Gender).Id;
                    employee.IdCheckpoint = employee.IdRole == 1 ? (CheckpointsComboBox.SelectedItem as Checkpoint).Id : null;
                    if (photoPath == null && !_changeMode)
                        employee.Photo = File.ReadAllBytes("/Resources/DefaultUserPhoto.jpg");
                    if (photoPath != null)
                        employee.Photo = File.ReadAllBytes(photoPath);

                    if (!_changeMode)
                        DbUtils.db.Employees.Add(employee);

                    DbUtils.db.SaveChanges();
                    Close();

                }
                catch(Exception ex)
                {
                    MessageBoxManager manager = new MessageBoxManager();
                    manager.Show("Ошибка сохранения", "Ошибка сохранения данных в базу");
                }
            }
        }

        private void RolesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RolesComboBox.SelectedItem is Role role && role.Id != 1)
            {
                CheckpointsComboBox.SelectedItem = null;
                CheckpointsComboBox.IsEnabled = false;
            }
            else
                CheckpointsComboBox.IsEnabled = true;
        }
    }
}
