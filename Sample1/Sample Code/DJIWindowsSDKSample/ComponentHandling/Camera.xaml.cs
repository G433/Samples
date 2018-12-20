using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using DJI.WindowsSDK;
using DJIWindowsSDKSample.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DJIWindowsSDKSample.ComponentHandling
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Camera : Page
    {
        public List<string> CameraShootPhotoModes => Enum.GetValues(typeof(CameraShootPhotoMode))
            .Cast<CameraShootPhotoMode>().Select(t => t.ToString()).ToList();

        public List<string> CameraWorkModes => Enum.GetValues(typeof(CameraWorkMode))
            .Cast<CameraWorkMode>().Select(t => t.ToString()).ToList();

        public List<string> CameraStorageLocation => Enum.GetValues(typeof(CameraStorageLocation))
            .Cast<CameraStorageLocation>().Select(t => t.ToString()).ToList();

        readonly CameraViewModel _cameraViewModel = new CameraViewModel();

        public Camera()
        {
            InitializeComponent();
            DataContext = _cameraViewModel;
        }

        public async void ShootPhotoModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            if(combo.SelectedItem == null)
                return;
            
            await _cameraViewModel.SetShootPhotoModeAsync(combo.SelectedItem.ToString()).ConfigureAwait(false);
        }

        public async void CameraWorkModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            if (combo.SelectedItem == null)
                return;

            await _cameraViewModel.SetCameraWorkModeAsync(combo.SelectedItem.ToString()).ConfigureAwait(false);
        }

        public void StorageLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            if (combo.SelectedItem == null)
                return;

            _cameraViewModel.StorageLocation = combo.SelectedItem.ToString();
        }
    }
}
