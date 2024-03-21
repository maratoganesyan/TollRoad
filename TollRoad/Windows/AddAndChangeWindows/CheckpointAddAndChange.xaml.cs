using Microsoft.Identity.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class CheckpointAddAndChange : UiWindow
    {
        int id;

        bool _changeMode;

        double _lat, _lng;

        List<PhotoOfCheckpoint> photosOfCheckPoint = new List<PhotoOfCheckpoint>();
        public CheckpointAddAndChange(Checkpoint checkpoint = null)
        {
            InitializeComponent();
            _changeMode = true;
            id = checkpoint.Id;
            _lat = Convert.ToDouble(checkpoint.Latitude);
            _lng = Convert.ToDouble(checkpoint.Longitude);
            AddressTextBox.Text = checkpoint.Address;
            PhotosView.ItemsSource = checkpoint.PhotoOfCheckpoints;
            MaxHeightTextBox.Text = checkpoint.MaxHeightOfVehicleInMm.ToString();
            MaxWidthTextBox.Text = checkpoint.MaxWidthOfVehicleInMm.ToString();
            photosOfCheckPoint = checkpoint.PhotoOfCheckpoints.ToList();
            PhotosView.ItemsSource = photosOfCheckPoint;
        }

        public CheckpointAddAndChange(double lat, double lng)
        {
            InitializeComponent();
            _lat = lat;
            _lng = lng;
            id = DbUtils.db.Checkpoints.Max(x => x.Id) + 1;
            SetAddress(lat, lng);


        }

        private async void SetAddress(double lat, double lng)
        {
            string address = await DbUtils.GetAddress(lat, lng);
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddressTextBox.Text = address;
            });
        }



        private void ChangePhoto_CLick(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
                PhotoOfCheckpoint photo = button.DataContext as PhotoOfCheckpoint;
                photo.Photo = File.ReadAllBytes(openFileDialog.FileName);
                photosOfCheckPoint[photo.NumberOfPhoto - 1] = photo;
                PhotosView.ItemsSource = null;
                PhotosView.ItemsSource = photosOfCheckPoint;
            }
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
                byte[] photo = File.ReadAllBytes(openFileDialog.FileName);
                PhotoOfCheckpoint photoOfCheckpoint = new PhotoOfCheckpoint();
                photoOfCheckpoint.IdCheckpoint = id;
                photoOfCheckpoint.NumberOfPhoto = photosOfCheckPoint.Count() + 1;
                photoOfCheckpoint.Photo = photo;
                photosOfCheckPoint.Add(photoOfCheckpoint);
                PhotosView.ItemsSource = null;
                PhotosView.ItemsSource = photosOfCheckPoint;

            }
        }

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            if (AddressTextBox.Text.Length == 0)
            {
                manager.Show("Ошибка сохранения", "Адрес не введен");
                return false;
            }
            if(!int.TryParse(MaxWidthTextBox.Text, out int _))
            {
                manager.Show("Ошибка сохранения", "Максимальная ширина автомобиля для прохода не введена");
                return false;
            }
            if (!int.TryParse(MaxHeightTextBox.Text, out int _))
            {
                manager.Show("Ошибка сохранения", "Максимальная высота автомобиля для прохода не введена");
                return false;
            }
            return true;
        }

        private void SavePhotos()
        {
            if (_changeMode)
            {
                foreach (var item in photosOfCheckPoint)
                {
                    PhotoOfCheckpoint photoOfCheckpoint = DbUtils.db.PhotoOfCheckpoints
                                        .FirstOrDefault(p => p.IdCheckpoint == item.IdCheckpoint && p.NumberOfPhoto == item.NumberOfPhoto);
                    if (photoOfCheckpoint == null)
                        DbUtils.db.Add(item);
                    else
                        photoOfCheckpoint.Photo = item.Photo;
                }
                DbUtils.db.SaveChanges();
            }
            else
            {
                foreach (var item in photosOfCheckPoint)
                {
                    DbUtils.db.Add(item);
                }
                DbUtils.db.SaveChanges();

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    Checkpoint checkpoint;
                    if (_changeMode)
                        checkpoint = DbUtils.db.Checkpoints.FirstOrDefault(e => e.Id == id);
                    else
                        checkpoint = new Checkpoint();

                    checkpoint.Latitude = Convert.ToDecimal(_lat);
                    checkpoint.Longitude = Convert.ToDecimal(_lng);
                    checkpoint.Address = AddressTextBox.Text;
                    checkpoint.MaxHeightOfVehicleInMm = int.Parse(MaxHeightTextBox.Text);
                    checkpoint.MaxWidthOfVehicleInMm = int.Parse(MaxWidthTextBox.Text);
                   
                    if (!_changeMode)
                        DbUtils.db.Checkpoints.Add(checkpoint);

                    DbUtils.db.SaveChanges();
                    SavePhotos();
                    Close();

                    if (!_changeMode)
                    {
                        MessageBoxManager manager = new MessageBoxManager();
                        int number = DbUtils.db.Checkpoints.Max(p => p.Id);
                        manager.Show("Успешное сохранение", $"КПП сохранен под номером {number}");
                    }

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
