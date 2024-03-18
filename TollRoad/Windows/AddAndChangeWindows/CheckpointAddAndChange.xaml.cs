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
            AddressTextBox.Text = checkpoint.Address;
            PhotosView.ItemsSource = checkpoint.PhotoOfCheckpoints;
            MaxHeightTextBox.Text = checkpoint.MaxHeightOfVehicleInMm.ToString();
            MaxWidthTextBox.Text = checkpoint.MaxWidthOfVehicleInMm.ToString();
            photosOfCheckPoint = checkpoint.PhotoOfCheckpoints.ToList();
        }

        public CheckpointAddAndChange(double lat, double lng)
        {
            InitializeComponent();
            _lat = lat;
            _lng = lng;
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

        private bool Validate()
        {
            MessageBoxManager manager = new MessageBoxManager();
            //if (FirstCheckpointComboBox.SelectedItem == null)
            //{
            //    manager.Show("Ошибка сохранения", "Первый КПП не выбран");
            //    return false;
            //}
            return true;
        }

        private void ChangePhoto_CLick(object sender, RoutedEventArgs e)
        {

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                byte[] photo = File.ReadAllBytes(openFileDialog.FileName);
                PhotoOfCheckpoint photo1  = new PhotoOfCheckpoint();
                //photo1.IdCheckpoint = 0;                                    СОСО ПАВИАШВИЛИ ТАМ 0 НАДО УБРАТЬ
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



                    if (!_changeMode)
                        DbUtils.db.Checkpoints.Add(checkpoint);

                    DbUtils.db.SaveChanges();
                    Close();

                    if(!_changeMode)
                    {
                        MessageBoxManager manager = new MessageBoxManager();
                        int number = DbUtils.db.Routs.OrderBy(e => e.Id).Last().Id;
                        manager.Show("Успешное сохранение", $"Маршрут сохранен под номером {number}");
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
