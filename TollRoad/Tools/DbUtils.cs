
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using TollRoad.Models;
using Wpf.Ui.Controls;
using Geocoding.Microsoft;
using Geocoding;

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

        public static async Task<string> GetAddress(double latitude, double longitude)
        {
            BingMapsGeocoder geocoder = new BingMapsGeocoder("An0KvRLGMUMOfUqeHxq3oipPsb61_0OXP75rO2N5Bj_kDDTXMc2Y-9AEQSK6iaHm");
            geocoder.Culture = "ru";
            var addresses = await geocoder.ReverseGeocodeAsync(latitude, longitude);
            var firstAddress = addresses?.FirstOrDefault();

            if (firstAddress != null)
            {
                return firstAddress.FormattedAddress;
            }
            else
            {
                // Обработка ошибки, если адрес не найден
                return "Адрес не найден";
            }
        }
    }
}
