using DJI.WindowsSDK;
using DJIUWPSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using DJIWindowsSDKSample.WinML;

namespace DJIWindowsSDKSample.ViewModels
{
    public class FpvViewModel : ViewModelBase
    {
        private MnistDigitRecognitionModel _modelGen;
        private readonly modelInput _modelInput = new modelInput();
        private modelOutput _modelOutput;
        private readonly Helpers.Helper _helper = new Helpers.Helper();

        public FpvViewModel()
        {
            LoadMachineLearningAsync();
        }
        
        public async Task AnalyzeFrameAsync(IBuffer buffer, uint width, uint height)
        {
            // Do not forget to dispose it! In this sample, we dispose it in ProcessSoftwareBitmap
            using (SoftwareBitmap bitmapToProcess = SoftwareBitmap.CreateCopyFromBuffer(buffer,
                BitmapPixelFormat.Bgra8, (int)width, (int)height, BitmapAlphaMode.Premultiplied))
            {
                _modelInput.Input3 =  await _helper.CropAndDisplayInputImage1Async(VideoFrame.CreateWithSoftwareBitmap(bitmapToProcess)).ConfigureAwait(false);
                if (_modelInput.Input3 == null)
                {
                    return;
                }

                //todo check the ConfigureAwait if should be true or false
                //Evaluate the model
                _modelOutput = await _modelGen.EvaluateAsync(_modelInput).ConfigureAwait(true);

                //Convert output to datatype
                IReadOnlyList<float> vectorImage = _modelOutput.Plus214_Output_0.GetAsVectorView();
                IList<float> imageList = vectorImage.ToList();

                //Query to check for highest probability digit
                var maxIndex = imageList.IndexOf(imageList.Max());

                if (maxIndex == 2 || maxIndex == 8 || maxIndex == 1)
                {
                    Debug.Write(maxIndex.ToString());
                }
            }

            //Display the results
            //numberLabel.Text = maxIndex.ToString();
        }

        public void UpdateJoystick(float throttle, float roll, float pitch, float yaw)
        {
            DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(throttle, roll, pitch, yaw);
        }

        public async Task<string> AutoTakeOffAsync()
        {
            var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                .StartTakeoffAsync().ConfigureAwait(true);
            var result = res.ToString();
            return result == SDKError.NO_ERROR.ToString() ? "Success" : result;
        }

        public async Task<string> AutoLandAsync()
        {
            var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                .StartAutoLandingAsync().ConfigureAwait(true);
            var result = res.ToString();
            return result == SDKError.NO_ERROR.ToString() ? "Success" : result;
        }

        private async void LoadMachineLearningAsync()
        {
            //Load a machine learning model
            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///WinML/MnistDigitRecognition.onnx"));
            _modelGen = await MnistDigitRecognitionModel.CreateFromStreamAsync(modelFile).ConfigureAwait(false);
        }
    }
}

// if need to work with BlockingCollection and constant moving sticks update consider the following code for that..

/*
 *using DJIDemo.Controls;
using DJISDK;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DJIDemo
{
    public sealed partial class MainPage : Page
    {
        DJIClient djiClient = DJIClient.Instance;
	
        public MainPage()
        {        
            this.InitializeComponent();
            viewModel = new MainPageViewModel(Dispatcher, djiClient);
        }

        private MainPageViewModel viewModel;
        public MainPageViewModel ViewModel => viewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Setup Joystick listener
            Task.Run(() => { ListenForJoystick(); });
        }

        #region Joystick Controls
        // Take off button
        private void ButtonTakeOff_Click(object sender, RoutedEventArgs e)
        {
            SetJoyStickValue(new JoyStickValues(-1, -1, -1, 1));
        }

        // Take off button in Landing mode
        private void ButtonLand_Click(object sender, RoutedEventArgs e)
        {
            SetJoyStickValue(new JoyStickValues(-1, 0, 0, 0));
        }

        // Go home button
        private void ButtonGoHome_Click(object sender, RoutedEventArgs e)
        {
            // todo:add gohome from djiclient
            bool goingHome = ((sender as CircularToggleButton).IsChecked == true);
        }

        private void JoystickLeft_OnJoystickReleased(object sender, JoystickEventArgs e)
        {
            Debug.WriteLine("JSL Released");
            SetJoyStickValue(new JoyStickValues(0, null, null, 0));
        }

        private void JoystickLeft_OnJoystickMoved(object sender, JoystickEventArgs e)
        {
            Debug.WriteLine("JSL Moved");
            SetJoyStickValue(new JoyStickValues(e.YValue, null, null, e.XValue));
        }

        private void JoystickRight_OnJoystickReleased(object sender, JoystickEventArgs e)
        {
            Debug.WriteLine("JSR Released");
            SetJoyStickValue(new JoyStickValues(null, 0, 0, null));
        }

        private void JoystickRight_OnJoystickMoved(object sender, JoystickEventArgs e)
        {
            Debug.WriteLine("JSR Moved");
            SetJoyStickValue(new JoyStickValues(null, e.XValue, e.YValue, null));
        }

        static BlockingCollection<JoyStickValues> JoyStickValuesQueue = new BlockingCollection<JoyStickValues>();

        private void SetJoyStickValue(JoyStickValues newValues)
        {
            JoyStickValuesQueue.TryAdd(newValues);
        }

        private void ListenForJoystick()
        {
            JoyStickValues current = new JoyStickValues(0, 0, 0, 0);
            foreach (var joystickItem in JoyStickValuesQueue.GetConsumingEnumerable())
            {
                current.throttle = joystickItem.throttle ?? current.throttle;
                current.roll = joystickItem.roll ?? current.roll;
                current.pitch = joystickItem.pitch ?? current.pitch;
                current.yaw = joystickItem.yaw ?? current.yaw;
                djiClient.SetJoyStickValue((float)current.throttle, (float)current.roll, (float)current.pitch, (float)current.yaw);
            }
        }
        #endregion //Joystick Controls

        private void JoystickLeft_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}


    using DJISDK;
using System;
using System.Runtime.InteropServices;
using Windows.Storage.Streams;

namespace DJIDemo
{
    static class Extensions
    {
        public static float? ConvertJoystickValue(this double? value)
        {
            if (value.HasValue)
                return (float?)(value * 100);
            return null;
        }
    }

    public class JoyStickValues
    {
        public JoyStickValues(double? throttle, double? roll, double? pitch, double? yaw)
        {
            this.throttle = throttle.ConvertJoystickValue();
            this.roll = roll.ConvertJoystickValue();
            this.pitch = pitch.ConvertJoystickValue();
            this.yaw = yaw.ConvertJoystickValue();
        }

        public float? throttle;
        public float? roll;
        public float? pitch;
        public float? yaw;
    }

    public sealed class DJIClient : IDisposable
    {
        private static volatile DJIClient instance;
        private static object syncRoot = new Object();
        private double velocityX, velocityY, velocityZ;
        private double pitch, yaw, roll;
        private double altitude;

        private DJIClient() { }

        public delegate void DJIBoolEventHandler(bool newValue);
        public delegate void DJIDoubleEventHandler(double newValue);
        public delegate void DJIAttitudeEventHandler(double pitch, double yaw, double roll);
        public delegate void DJIVelocityEventHandler(double X, double Y, double Z);
        public event DJIBoolEventHandler FlyingChanged;
        public event DJIBoolEventHandler ConnectedChanged;
        public event DJIDoubleEventHandler AltitudeChanged;
        public event DJIAttitudeEventHandler AttitudeChanged;
        public event DJIVelocityEventHandler VelocityChanged;
        DJIClientNative.BoolCallback flyingCallback;
        DJIClientNative.BoolCallback connectedCallback;
        DJIClientNative.DoubleCallback altitudeCallback;
        DJIClientNative.AttitudeCallback attitudeCallback;
        DJIClientNative.VelocityCallback velocityHandler;
        DJIClientNative.VideoFrameBufferCallback videoFrameBufferCallback;

        public delegate void VideoEventHandler(IBuffer buffer, uint width, uint height, ulong timeStamp);
        public event VideoEventHandler FrameArived;

        public static DJIClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DJIClient();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Start the connection to the DJI Drone. It is advised to attach handlers to events prior to calling this method.
        /// </summary>
        public void Initialize()
        {
            flyingCallback = Instance.OnFlyingChanged;
            connectedCallback = Instance.OnConnected;
            altitudeCallback = Instance.OnAltitudeChanged;
            attitudeCallback = Instance.OnAttitudeChanged;
            velocityHandler = Instance.OnVelocityChanged;
            videoFrameBufferCallback = Instance.OnVideoData;

            DJIClientNative.InitializeDJISDK(
                videoFrameBufferCallback,
                connectedCallback, 
                flyingCallback, 
                altitudeCallback, 
                attitudeCallback,
                velocityHandler);
        }

        private void OnVelocityChanged(double X, double Y, double Z)
        {
            velocityX = X;
            velocityY = Y;
            velocityZ = Z;
            VelocityChanged?.Invoke(velocityX, velocityY, velocityZ);
        }

        private void OnAttitudeChanged(double pitch, double yaw, double roll)
        {
            this.pitch = pitch;
            this.yaw = yaw;
            this.roll = roll;
            AttitudeChanged?.Invoke(pitch, yaw, roll);
        }

        private void OnAltitudeChanged(double newValue)
        {
            altitude = newValue;
            AltitudeChanged?.Invoke(altitude);
        }

        public void UnInitialize()
        {
            DJIClientNative.UninitializeDJISDK();
        }

        public bool IsFlying { get; private set; } = false;

        public bool IsConnected { get; private set; } = false;

        public void SetJoyStickValue(float throttle, float roll, float pitch, float yaw)
        {
            DJIClientNative.SetJoyStickValue(throttle, roll, pitch, yaw);
        }

        public void SetGimbleAngle(double pitch, double yaw = 0, double roll = 0, bool pitchControlInvalid = false, bool rollControlInvalid = false, bool yawControlInvalid = false, double time = 1, double mode = 1)
        {
            DJIClientNative.SetGimbleAngle(pitch, yaw, roll, pitchControlInvalid, rollControlInvalid, yawControlInvalid, time, mode);
        }

        private void OnFlyingChanged(bool flying)
        {
            if (flying != IsFlying)
            {
                IsFlying = flying;
                FlyingChanged?.Invoke(flying);
            }
        }

        private void OnVideoData(IntPtr frameIBufferPtr, uint width, uint height, ulong timeStamp)
        {
            IBuffer buffer = Marshal.GetObjectForIUnknown(frameIBufferPtr) as IBuffer;
            FrameArived?.Invoke(buffer, width, height, timeStamp);
        }

        private void OnConnected(bool connected)
        {
            if (connected != IsConnected)
            {
                IsConnected = connected;
                ConnectedChanged?.Invoke(connected);
            }
        }

        public void Dispose()
        {
            UnInitialize();
        }
    }
}

 *
 *
 */

