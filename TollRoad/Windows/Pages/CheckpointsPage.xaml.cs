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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.IO;
using Wpf.Ui.Controls;
using TollRoad.Tools;
using TollRoad.Models;

namespace TollRoad.Windows.Pages
{
    /// <summary>
    /// Interaction logic for CheckpointsPage.xaml
    /// </summary>
    public partial class CheckpointsPage : Page
    {
        double lat;

        double lng;
        public CheckpointsPage()
        {
            InitializeComponent();

        }

        private void gMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            gMapControl.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl.Position = new PointLatLng(55.755829, 37.617627); // Координаты центра карты

            // Настройка отображения
            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 17;
            gMapControl.Zoom = 10;
            gMapControl.MouseWheelZoomEnabled = true;

            gMapControl.IgnoreMarkerOnMouseWheel = true;
            gMapControl.DragButton = MouseButton.Left;

            // Добавление маркера
            var Checkpoints = DbUtils.db.Checkpoints.ToList();
            foreach (var coor in Checkpoints)
            {
                GMapMarker marker = new GMapMarker(new PointLatLng(Convert.ToDouble(coor.Latitude), Convert.ToDouble(coor.Longitude)));
                Image image = new Image();
                image.Visibility = Visibility.Visible;
                image.Source = MapIcon.Source;
                image.Tag = coor.Id;
                image.Height = 30;
                marker.Shape = image;
                marker.Shape.MouseLeftButtonDown += Marker_MouseLeftButtonDown;

                gMapControl.Markers.Add(marker);
            }
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var markerShape = sender as Image;
            var checkpoint = DbUtils.db.Checkpoints.FirstOrDefault(p => p.Id == Convert.ToInt32(markerShape.Tag));
            CheckPointInfo.DataContext = checkpoint;
            CheckPointInfo.Visibility = Visibility.Visible;
            
        }

        private void gMapControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var points = e.GetPosition(this);
            lat = gMapControl.FromLocalToLatLng(Convert.ToInt32(points.X) - 15, Convert.ToInt32(points.Y) - 35).Lat;
            lng = gMapControl.FromLocalToLatLng(Convert.ToInt32(points.X) - 15, Convert.ToInt32(points.Y) - 35).Lng;
           
        }

        private void AddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            //GMapMarker marker = new GMapMarker(new PointLatLng(lat, lng));
            //Image image = new Image();
            //image.Visibility = Visibility.Visible;
            //image.Source = MapIcon.Source;
            //image.Height = 30;
            //marker.Shape = image;
            //marker.Shape.MouseLeftButtonDown += Marker_MouseLeftButtonDown;

            //gMapControl.Markers.Add(marker);
        }
    }
}
