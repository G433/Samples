using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using DJI.WindowsSDK;
using DJIUWPSample.Commands;
using DJIUWPSample.ViewModels;

namespace DJIWindowsSDKSample.ViewModels
{
    public class CameraViewModel : ViewModelBase
    {
        private readonly CoreDispatcher _dispatcher;

        public CameraViewModel()
        {
            _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).IsTakingPhotoChanged += IsTakingPhoto;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).PhotoIntervalShootSettingsChanged += PhotoIntervalShoot;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).ShootPhotoModeChanged += ShootPhotoModeChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).CameraWorkModeChanged += CameraWorkModeChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).IsRecordingChanged += IsRecordingChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).RecordingTimeChanged += RecordingTimeChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).VideoResolutionAndFrameRateChanged += VideoResolutionAndFrameRateChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).NewlyGeneratedMediaFileChanged += NewlyGeneratedMediaFileChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).SDCardRemainSpaceChanged += SdCardRemainSpaceChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).SDCardAvailablePhotoCountChanged += SdCardAvailablePhotoCountChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).SDCardAvailableVideoDurationChanged += SdCardAvailableVideoDurationChanged;
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).CameraSDCardStateChanged += CameraSdCardStateChanged;
        }

        private string _cameraSdCardState;

        public string CameraSdCardState
        {
            get => _cameraSdCardState;
            set
            {
                _cameraSdCardState = value;
                OnPropertyChanged();
            }
        }

        private async void CameraSdCardStateChanged(object sender, CameraSDCardStateMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => CameraSdCardState = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private int _sDCardAvailableVideoDuration;

        public int SdCardAvailableVideoDuration
        {
            get => _sDCardAvailableVideoDuration;
            set
            {
                _sDCardAvailableVideoDuration = value;
                OnPropertyChanged();
            }
        }

        private async void SdCardAvailableVideoDurationChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SdCardAvailableVideoDuration = value.Value.value).ConfigureAwait(true);
        }

        private int _sDCardAvailablePhoto;

        public int SdCardAvailablePhoto
        {
            get => _sDCardAvailablePhoto;
            set
            {
                _sDCardAvailablePhoto = value;
                OnPropertyChanged();
            }
        }

        private async void SdCardAvailablePhotoCountChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SdCardAvailablePhoto = value.Value.value).ConfigureAwait(true);
        }

        private int _sDCardRemainSpace;

        public int SdCardRemainSpace
        {
            get => _sDCardRemainSpace;
            set
            {
                _sDCardRemainSpace = value;
                OnPropertyChanged();
            }
        }

        private async void SdCardRemainSpaceChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SdCardRemainSpace = value.Value.value).ConfigureAwait(true);
        }

        public string VideoResolution { get; set; }
        public string VideoFrameRate { get; set; }

        private VideoResolutionAndFrameRate _videoResolutionAndFrameRate;

        public VideoResolutionAndFrameRate VideoResolutionAndFrameRate
        {
            get => _videoResolutionAndFrameRate;
            set
            {
                _videoResolutionAndFrameRate = value;
                OnPropertyChanged($"VideoResolution");
                OnPropertyChanged($"VideoFrameRate");
            }
        }

        private async void VideoResolutionAndFrameRateChanged(object sender, VideoResolutionAndFrameRate? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => VideoResolutionAndFrameRate = value.Value).ConfigureAwait(true);
        }


        private int _recordingTime;

        public int RecordingTime
        {
            get => _recordingTime;
            set
            {
                _recordingTime = value;
                OnPropertyChanged();
            }
        }

        private async void RecordingTimeChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RecordingTime = value.Value.value).ConfigureAwait(true);
        }

        private bool _isRecordingVideo;

        public bool IsRecordingVideo
        {
            get => _isRecordingVideo;
            set
            {
                _isRecordingVideo = value;
                OnPropertyChanged();
            }
        }

        private async void IsRecordingChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsRecordingVideo = value.Value.value).ConfigureAwait(true);
        }

        public string MediaFileFormat => MediaFileInfo.type.ToString();
        public int MediaFileSize => MediaFileInfo.fileSize;
        public int MediaFileIndex => MediaFileInfo.index;

        //public int MediaFileTimeCreated => MediaFileInfo.timeCreated; 

        private GeneratedMediaFileInfo _mediaFileInfo;

        public GeneratedMediaFileInfo MediaFileInfo
        {
            get => _mediaFileInfo;
            set
            {
                _mediaFileInfo = value;
                OnPropertyChanged($"MediaFileFormat");
                OnPropertyChanged($"MediaFileSize");
                OnPropertyChanged($"MediaFileIndex");
                OnPropertyChanged($"MediaFileTimeCreated");
            }
        }

        private async void NewlyGeneratedMediaFileChanged(object sender, GeneratedMediaFileInfo? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => MediaFileInfo = value.Value).ConfigureAwait(true);
        }

        private string _cameraWorkMode;

        public string CameraWorkMode
        {
            get => _cameraWorkMode;
            set
            {
                _cameraWorkMode = value;
                OnPropertyChanged();
            }
        }

        private async void CameraWorkModeChanged(object sender, CameraWorkModeMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => CameraWorkMode = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private string _shootPhotoMode;

        public string ShootPhotoMode
        {
            get => _shootPhotoMode;
            set
            {
                _shootPhotoMode = value;
                OnPropertyChanged();
            }
        }

        private async void ShootPhotoModeChanged(object sender, CameraShootPhotoModeMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => ShootPhotoMode = value.Value.value.ToString()).ConfigureAwait(true);
        }

        public int PhotoIntervalShootSettingsInterval => PhotoIntervalShootSettings.interval;

        public int PhotoIntervalShootSettingsCount => PhotoIntervalShootSettings.count;

        private PhotoIntervalShootSettings _photoIntervalShootSettings;

        public PhotoIntervalShootSettings PhotoIntervalShootSettings
        {
            get => _photoIntervalShootSettings;
            set
            {
                _photoIntervalShootSettings = value;
                OnPropertyChanged($"PhotoIntervalShootSettingsInterval");
                OnPropertyChanged($"PhotoIntervalShootSettingsCount");
            }
        }

        private async void PhotoIntervalShoot(object sender, PhotoIntervalShootSettings? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => PhotoIntervalShootSettings = value.Value).ConfigureAwait(true);
        }

        private bool _isCurrentlyTakingPhoto;

        public bool IsCurrentlyTakingPhoto
        {
            get => _isCurrentlyTakingPhoto;
            set
            {
                _isCurrentlyTakingPhoto = value;
                OnPropertyChanged();
            }
        }

        private async void IsTakingPhoto(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsCurrentlyTakingPhoto = value.Value.value).ConfigureAwait(true);
        }

        private string _shootPhotoStatus;

        public string ShootPhotoStatus
        {
            get => _shootPhotoStatus;
            set
            {
                _shootPhotoStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand StartShootPhotoCommand;
        public ICommand StartShootPhoto
        {
            get
            {
                return StartShootPhotoCommand ?? (StartShootPhotoCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .StartShootPhotoAsync().ConfigureAwait(true);
                   ShootPhotoStatus = res.ToString();
                }, () => true));
            }
        }

        private string _stopShootPhotoStatus;

        public string StopShootPhotoStatus
        {
            get => _stopShootPhotoStatus;
            set
            {
                _stopShootPhotoStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand StopShootPhotoCommand;
        public ICommand StopShootPhoto
        {
            get
            {
                return StopShootPhotoCommand ?? (StopShootPhotoCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .StopShootPhotoAsync().ConfigureAwait(true);
                    StopShootPhotoStatus = res.ToString();
                }, () => true));
            }
        }

        private string _recordStatus;

        public string RecordStatus
        {
            get => _recordStatus;
            set
            {
                _recordStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand StartRecordCommand;
        public ICommand StartRecord
        {
            get
            {
                return StartRecordCommand ?? (StartRecordCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .StartRecordAsync().ConfigureAwait(true);
                    RecordStatus = res.ToString();
                }, () => true));
            }
        }

        private string _stopRecordStatus;

        public string StopRecordStatus
        {
            get => _stopRecordStatus;
            set
            {
                _stopRecordStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand StopRecordCommand;
        public ICommand StopRecord
        {
            get
            {
                return StopRecordCommand ?? (StopRecordCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .StopRecordAsync().ConfigureAwait(true);
                    StopRecordStatus = res.ToString();
                }, () => true));
            }
        }

        private string _getSdCardAvailableVideoDurationResult;

        public string GetSdCardAvailableVideoDurationResult
        {
            get => _getSdCardAvailableVideoDurationResult;
            set
            {
                _getSdCardAvailableVideoDurationResult = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand GetSdCardAvailableVideoDurationCommand;
        public ICommand GetSdCardAvailableVideoDuration
        {
            get
            {
                return GetSdCardAvailableVideoDurationCommand ?? (GetSdCardAvailableVideoDurationCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .GetSDCardAvailableVideoDurationAsync().ConfigureAwait(true);
                    GetSdCardAvailableVideoDurationResult = res.ToString();
                }, () => true));
            }
        }

        private string _sDCardAvailablePhotoCount;

        public string SdCardAvailablePhotoCount
        {
            get => _sDCardAvailablePhotoCount;
            set
            {
                _sDCardAvailablePhotoCount = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetSdCardAvailablePhotoCountCommand;
        public ICommand GetSdCardAvailablePhotoCount
        {
            get
            {
                return GetSdCardAvailablePhotoCountCommand ?? (GetSdCardAvailablePhotoCountCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).GetSDCardAvailablePhotoCountAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        SdCardAvailablePhotoCount = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        SdCardAvailablePhotoCount = res.error.ToString();
                    }
                }, () => true));
            }
        }

        private string _photoIntervalStatus;

        public string PhotoIntervalShootStatus
        {
            get => _photoIntervalStatus;
            set
            {
                _photoIntervalStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public int PhotoIntervalShootInterval { get; set; }
        public int PhotoIntervalShootCount { get; set; }

        public ICommand SetPhotoIntervalShootSettingsCommand;
        public ICommand SetPhotoIntervalShootSettings
        {
            get
            {
                return SetPhotoIntervalShootSettingsCommand ?? (SetPhotoIntervalShootSettingsCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .SetPhotoIntervalShootSettingsAsync(new PhotoIntervalShootSettings{count = PhotoIntervalShootCount, interval = PhotoIntervalShootInterval}).ConfigureAwait(true);
                    PhotoIntervalShootStatus = res.ToString();
                }, () => true));
            }
        }

        private string _setShootPhotoModeStatus;

        public string SetShootPhotoModeStatus
        {
            get => _setShootPhotoModeStatus;
            set
            {
                _setShootPhotoModeStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value; 
                OnPropertyChanged();
            }
        }

        public async Task SetShootPhotoModeAsync(string mode)
        {
            var shootingMode = (CameraShootPhotoMode)Enum.Parse(typeof(CameraShootPhotoMode), mode);
            var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                .SetShootPhotoModeAsync(new CameraShootPhotoModeMsg { value = shootingMode }).ConfigureAwait(true);
            SetShootPhotoModeStatus = res.ToString();
        }

        private string _setCameraWorkModeStatus;

        public string SetCameraWorkModeStatus
        {
            get => _setCameraWorkModeStatus;
            set
            {
                _setCameraWorkModeStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value; 
                OnPropertyChanged();
            }
        }

        public async Task SetCameraWorkModeAsync(string cameraWorkMode)
        {
            var workMode = (CameraWorkMode)Enum.Parse(typeof(CameraWorkMode), cameraWorkMode);
            var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                .SetCameraWorkModeAsync(new CameraWorkModeMsg { value = workMode }).ConfigureAwait(true);
            SetCameraWorkModeStatus = res.ToString();
        }

        private string _formatStorageStatus;

        public string FormatStorageStatus
        {
            get => _formatStorageStatus;
            set
            {
                _formatStorageStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public string StorageLocation { get; set; }

        public ICommand FormatStorageCommand;
        public ICommand FormatStorage
        {
            get
            {
                return FormatStorageCommand ?? (FormatStorageCommand = new RelayCommand(async delegate
                {
                    var storageLocation = (CameraStorageLocation)Enum.Parse(typeof(CameraStorageLocation), StorageLocation);
                    var res = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0)
                        .FormatStorageAsync(new CameraStorageLocationMsg{ value = storageLocation }).ConfigureAwait(true);
                    FormatStorageStatus = res.ToString();
                }, () => true));
            }
        }

        private async Task OnUiThreadAsync(Action action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await _dispatcher.RunAsync(priority, () => action());
        }
    }
}
