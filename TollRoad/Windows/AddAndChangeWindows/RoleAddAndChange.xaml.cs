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
    public partial class RoleAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public RoleAddAndChange(bool changeMode, Role role = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            if(changeMode)
            {
                id = role.Id;
                RoleNameTextBox.Text = role.RoleName;
            }
        }

        private bool Validation()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (RoleNameTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения","Поля не заполнены");
                return false;
            }
            if (DbUtils.db.Roles.ToList().Any(ce => ce.RoleName == RoleNameTextBox.Text && ce.Id != id))
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
                Role role;
                if (_changeMode)
                    role = DbUtils.db.Roles.FirstOrDefault(c => c.Id == id);
                else
                    role = new Role();

                role.RoleName = RoleNameTextBox.Text;

                if (!_changeMode)
                    DbUtils.db.Roles.Add(role);

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
