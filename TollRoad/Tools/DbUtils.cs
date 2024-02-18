using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollRoad.Models;
using Wpf.Ui.Controls;

namespace TollRoad.Tools
{
    static class DbUtils
    {
        public static Db db;

        static DbUtils()
        {
            try
            {
                db = new Db();
            }
            catch(Exception e)
            {
                MessageBox box = new MessageBox();
                box.Content = "Ошибка подключения к базе";
            }
        }
    }
}
