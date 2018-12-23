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
