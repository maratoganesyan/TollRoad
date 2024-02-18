using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace TollRoad.Tools
{
    class MessageBoxManager
    {
        private MessageBox box;

        public MessageBoxManager()
        {
            box = new MessageBox();
            box.ButtonLeftName = "Ок";
            box.ButtonRightName = "Закрыть";
            box.ButtonRightClick += Box_ButtonClick;
            box.ButtonLeftClick += Box_ButtonClick;

        }

        private void Box_ButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            box.Close();
        }

        public void Show(string title, string content)
        {
            box.Show(title, content);
        }

    }
}
