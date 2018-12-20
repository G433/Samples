using DJI.WindowsSDK;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using DJIWindowsSDKSample.ViewModels;

namespace DJIWindowsSDKSample.FPV
{
    // ReSharper disable once InconsistentNaming
    public sealed partial class FPVPage : Page
    {
        private DJIVideoParser.Parser _videoParser;
        public WriteableBitmap VideoSource;
        private byte[] _decodedDataBuf;
        private readonly object _bufLock = new object();
        private readonly VirtualRemoteControllerViewModel _remoteControllerViewModelModel = new VirtualRemoteControllerViewModel();
        private float _yaw;
        private float _roll;
        private float _pitch;
        private float _throttle;

        public FPVPage()
        {
            InitializeComponent();
            DataContext = _remoteControllerViewModelModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            InitializeVideoFeedModule();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            UninitializeVideoFeedModule();
        }


        private void InitializeVideoFeedModule()
        {
            _videoParser = new DJIVideoParser.Parser();
            _videoParser.Initialize();
            _videoParser.SetVideoDataCallack(0, 0, ReceiveDecodedData);
            if (DJISDKManager.Instance.SDKRegistrationResultCode == SDKError.NO_ERROR)
            {
                DJISDKManager.Instance.VideoFeeder.GetPrimaryVideoFeed(0).VideoDataUpdated += OnVideoPush;
            }
        }

        private void UninitializeVideoFeedModule()
        {
            if (DJISDKManager.Instance.SDKRegistrationResultCode == SDKError.NO_ERROR)
            {
                _videoParser.SetVideoDataCallack(0, 0, null);
                DJISDKManager.Instance.VideoFeeder.GetPrimaryVideoFeed(0).VideoDataUpdated -= OnVideoPush;
            }
        }

        void OnVideoPush(VideoFeed sender, [ReadOnlyArray] ref byte[] bytes)
        {
            _videoParser.PushVideoData(0, 0, bytes, bytes.Length);
        }

        async void ReceiveDecodedData(byte[] data, int width, int height)
        {
            lock (_bufLock)
            {
                if (_decodedDataBuf == null)
                {
                    _decodedDataBuf = data;
                }
                else
                {
                    data.CopyTo(_decodedDataBuf.AsBuffer());
                }
            }
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (VideoSource == null || VideoSource.PixelWidth != width || VideoSource.PixelHeight != height)
                {
                    VideoSource = new WriteableBitmap(width, height);
                    fpvImage.Source = VideoSource;
                }

                lock (_bufLock)
                {
                    _decodedDataBuf.AsBuffer().CopyTo(VideoSource.PixelBuffer);
                }
                VideoSource.Invalidate();
            });
        }

        private void Slider1_ThrottleValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is Slider slider)
            {
                _throttle = (float)slider.Value;
                _remoteControllerViewModelModel.UpdateJoystick(_throttle, _roll, _pitch, _yaw);
            }
        }

        private void Slider2_RollValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is Slider slider)
            {
                _roll = (float)slider.Value;
                _remoteControllerViewModelModel.UpdateJoystick(_throttle, _roll, _pitch, _yaw);
            }
        }

        private void Slider3_PitchValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is Slider slider)
            {
                _pitch = (float)slider.Value;
                _remoteControllerViewModelModel.UpdateJoystick(_throttle, _roll, _pitch, _yaw);
            }
        }

        private void Slider4_YawValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender is Slider slider)
            {
                _yaw = (float)slider.Value;
                _remoteControllerViewModelModel.UpdateJoystick(_throttle, _roll, _pitch, _yaw);
            }
        }

        private void Button_Hover1Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick();
            UpdateJoyStickSlidersValues();
        }

        private void Button_Hover2Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick();
        }

        private void ForwardButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle:(float) 0.0, roll: 0, pitch: (float)0.4, yaw: 0);
        }

        private void BackwardButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle: (float)0.0, roll: 0, pitch: (float)-0.4, yaw: 0);
        }

        private void RightButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle: 0, roll: 0, pitch: 0, yaw: (float)0.4); //yaw and roll mixed change when sdk will be fixed
        }

        private void LeftButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle: 0, roll: 0, pitch: 0, yaw: (float)-0.4); //yaw and roll mixed change when sdk will be fixed
        }

        private void RightYawButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle: (float)0.0, roll: (float)0.4, pitch: 0, yaw: 0); //yaw and roll mixed change when sdk will be fixed
        }

        private void LeftYawButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(throttle: (float)0.0, roll: (float)-0.4, pitch: 0, yaw: 0); //yaw and roll mixed change when sdk will be fixed
        }

        private async void TakeOff_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StatusTextBlock.Text = await _remoteControllerViewModelModel.AutoTakeOffAsync().ConfigureAwait(true);
            UpdateJoyStickCachedValues();
        }

        private async void Land_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateJoyStick(); //hover
            await Task.Delay(2000).ConfigureAwait(true);
            StatusTextBlock.Text = await _remoteControllerViewModelModel.AutoLandAsync().ConfigureAwait(true);
        }

        private void UpdateJoyStick(float throttle = 0, float roll = 0, float pitch = 0, float yaw = 0)
        {
            _remoteControllerViewModelModel.UpdateJoystick(throttle, roll, pitch, yaw);
            UpdateJoyStickCachedValues(throttle, roll, pitch, yaw);
        }

        private void UpdateJoyStickCachedValues(float throttle = 0, float roll = 0, float pitch = 0, float yaw = 0)
        {
             _throttle = throttle;
            _roll = roll;
            _pitch = pitch;
            _yaw = yaw;
        }

        private void UpdateJoyStickSlidersValues(float throttle = 0, float roll = 0, float pitch = 0, float yaw = 0)
        {
            SliderThrottle.Value = throttle;
            SliderRoll.Value = roll;
            SliderPitch.Value = pitch;
            SliderYaw.Value = yaw;
        }
    }
}
