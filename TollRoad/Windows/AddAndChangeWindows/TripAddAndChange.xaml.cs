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
    public partial class TripAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;
        public TripAddAndChange(bool changeMode, Trip trip = null)
        {
            InitializeComponent();
            _changeMode = changeMode;
            RouteComboBox.ItemsSource = DbUtils.db.Routs.ToList();
            VehicleComboBox.ItemsSource = DbUtils.db.Vehicles.ToList();
            StatusComboBox.ItemsSource = DbUtils.db.StatusesOfTrips.ToList();
            if(changeMode)
            {
                id = trip.Id;
                StartDateTime.Value = trip.StartDateTimeOfTrip;
                EndDateTime.Value = trip.EndDateTimeOfTrip;
                RouteComboBox.SelectedItem = trip.IdRouteNavigation;
                VehicleComboBox.SelectedItem = trip.IdVehicleNavigation;
                StatusComboBox.SelectedItem = trip.StatusOfTripNavigation;
            }
        }

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (!StartDateTime.Value.HasValue)
            {
                manager.Show("Ошибка сохранения", "Дата конца поездки не выбрана");
                return false;
            }
            if (!EndDateTime.Value.HasValue)
            {
                manager.Show("Ошибка сохранения", "Дата конца поездки не выбрана");
                return false;
            }
            if(StartDateTime.Value.Value >= EndDateTime.Value.Value)
            {
                manager.Show("Ошибка сохранения", "Дата начала поездки меньше даты конца");
                return false;
            }
            if(RouteComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Маршрут не выбран");
                return false;
            }
            if (VehicleComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Машина не выбрана");
                return false;
            }
            if (StatusComboBox.SelectedItem == null)
            {
                manager.Show("Ошибка сохранения", "Статус поездки не выбран");
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
                    Trip trip;
                    if (_changeMode)
                        trip = DbUtils.db.Trips.FirstOrDefault(e => e.Id == id);
                    else
                        trip = new Trip();

                    trip.EndDateTimeOfTrip = EndDateTime.Value.Value;
                    trip.StartDateTimeOfTrip = StartDateTime.Value.Value;
                    trip.StatusOfTrip = (StatusComboBox.SelectedItem as StatusesOfTrip).Id;
                    trip.IdVehicle = (VehicleComboBox.SelectedItem as Vehicle).Id;
                    trip.IdRoute = (RouteComboBox.SelectedItem as Rout).Id;
                    trip.TotalPriceOfTrip = Math.Round(decimal.Parse(Price.Text), 2);

                    if (!_changeMode)
                        DbUtils.db.Trips.Add(trip);

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

        private void RouteOrVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RouteComboBox.SelectedItem == null || VehicleComboBox.SelectedItem == null)
                return;
            else
                Price.Text = ((RouteComboBox.SelectedItem as Rout).Fare * (VehicleComboBox.SelectedItem as Vehicle).CategoryOfVehicleNavigation.FaceCoefficient).ToString();
        }
    }
}
