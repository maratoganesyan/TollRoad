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
using TollRoad.Tools;
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;

namespace TollRoad.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : UiWindow
    {
        int attemptCount;

        string answerForCaptcha;
        public AuthWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Transitions.ApplyTransition(this, TransitionType.FadeInWithSlide, 1000);
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if(DbUtils.db.Employees.Any(u => u.Login == LoginTextBox.Text && u.Password == PasswordBox.Password))
            {
                var employee = DbUtils.db.Employees.Single(u => u.Login == LoginTextBox.Text && u.Password == PasswordBox.Password);
                new EmployeeWindow(employee).Show();
                this.Close();
            }
            else
            {
                attemptCount++;
                if(attemptCount == 3)
                {
                    GenerateCaptcha();
                    CaptchaDialog.Show();
                }
                else
                {
                    new MessageBoxManager().Show("Ошибка авторизации", "Логин или пароль введены не верно.\n Проверьте введённые данные");
                }
            }
        }

        private void GenerateCaptcha()
        {
            CaptchaDialog.Show();
            Captcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 6);
            answerForCaptcha = Captcha.CaptchaText;
            AnswwerTextBox.Text = string.Empty;
        }

        private void CaptchaDialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            if (AnswwerTextBox.Text == answerForCaptcha)
            {
                CaptchaDialog.Hide();
                attemptCount = 0;
            }
            else
                GenerateCaptcha();
        }

        private void CaptchaDialog_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
    }
}
