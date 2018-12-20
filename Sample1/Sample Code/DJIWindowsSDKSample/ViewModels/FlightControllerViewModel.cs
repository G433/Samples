using DJI.WindowsSDK;
using DJIUWPSample.Commands;
using DJIUWPSample.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace DJIWindowsSDKSample.ViewModels
{
    public class FlightControllerViewModel : ViewModelBase
    {
        private readonly CoreDispatcher _dispatcher;

        public FlightControllerViewModel()
        {
            _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            //real time flight information
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).FlightTimeInSecondsChanged += FlightTimeInSecondsChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RemainingFlightTimeChanged += RemainingFlightTimeChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).SatelliteCountChanged += SatelliteCountChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GPSSignalLevelChanged += GpsSignalLevelChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AircraftLocationChanged += AircraftLocationChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).VelocityChanged += VelocityChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AltitudeChanged += AltitudeChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AttitudeChanged += AttitudeChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AreMotorsOnChanged += AreMotorsOnChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsIMUWarmingUpChanged += IsImuWarmingUpChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsFlyingChanged += IsFlyingChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).HomeLocationChanged += HomeLocationChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AutoRTHReasonChanged += AutoRthReasonChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsHomeLocationSetChanged += IsHomeLocationSetChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).TimeNeededToGoHomeChanged += TimeNeededToGoHomeChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).ShouldGoHomeChanged += ShouldGoHomeChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RequireGoHomeByLowBatteryVoltageChanged += RequireGoHomeByLowBatteryVoltageChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RequireGoHomeBySmartBatteryPercentChanged += RequireGoHomeBySmartBatteryPercentChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).SmartRTHCountdownChanged += SmartRthCountdownChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GoHomeStateChanged += GoHomeStateChanged;

            //landing
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).TimeNeededToLandChanged += TimeNeededToLandChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).BatteryPercentNeededToLandChanged += BatteryPercentNeededToLandChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RequireLandingByLowBatteryVoltageChanged += RequireLandingByLowBatteryVoltageChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RequireLandingBySmartBatteryPercentChanged += RequireLandingBySmartBatteryPercentChanged;

            //battery
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).LowBatteryWarningThresholdChanged += LowBatteryWarningThresholdChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).SeriousLowBatteryWarningThresholdChanged += SeriousLowBatteryWarningThresholdChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsLowBatteryWarningChanged += IsLowBatteryWarningChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsSeriousLowBatteryWarningChanged += IsSeriousLowBatteryWarningChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).BatteryThresholdBehaviorChanged += BatteryThresholdBehaviorChanged;

            //warnings
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).HasNoEnoughForceChanged += HasNotEnoughForceChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GPSModeFailureReasonChanged += GpsModeFailureReasonChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).WindWarningChanged += WindWarningChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).MotorStartFailureErrorChanged += MotorStartFailureErrorChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsMotorStuckChanged += IsMotorStuckChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).NoPropellerInstalledChanged += NoPropellerInstalledChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).HasIceOnPropellersChanged += HasIceOnPropellersChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsESCDisconnectedChanged += IsEscDisconnectedChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).ESCHasErrorChanged += EscHasErrorChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).CompassHasErrorChanged += CompassHasErrorChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).CompassInstallErrorChanged += CompassInstallErrorChanged;

            //take off
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).TakeoffLocationAltitudeChanged += TakeoffLocationAltitudeChanged;

            //settings
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).FlightModeChanged += FlightModeChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).FCRemoteControllerSwitchModeChanged += FcRemoteControllerSwitchModeChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).FailsafeActionChanged += FailSafeActionChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsVisionSensorUsedChanged += IsVisionSensorUsedChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AircraftNameChanged += AircraftNameChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsLandingConfirmationNeededChanged += IsLandingConfirmationNeededChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GroundStationModeEnabledChanged += GroundStationModeEnabledChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsNearHeightLimitChanged += IsNearHeightLimitChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsNearDistanceLimitChanged += IsNearDistanceLimitChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GoHomeHeightChanged += GoHomeHeightChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).MaxRadiusCanFlyAndGoHomeChanged += MaxRadiusCanFlyAndGoHomeChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).HeightLimitChanged += HeightLimitChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).DistanceLimitChanged += DistanceLimitChanged;

            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsCompassCalibratingChanged += IsCompassCalibratingChanged;
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).CompassCalibrationStateChanged += CompassCalibrationStateChanged;


            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).BatteryPercentNeededToGoHomeChanged += BatteryPercentNeededToGoHomeChanged;

            //novice mode
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).NoviceModeEnabledChanged += NoviceModeEnabledChanged;
            //simulator
            DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsSimulatorStartedChanged += IsSimulatorStartedChanged;
        }

        private string _goHomeState;

        public string GoHomeState
        {
            get => _goHomeState;
            set
            {
                _goHomeState = value;
                OnPropertyChanged();
            }
        }

        private async void GoHomeStateChanged(object sender, FCGoHomeStateMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => GoHomeState = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _eScHasError;

        public bool EscHasError
        {
            get => _eScHasError;
            set
            {
                _eScHasError = value;
                OnPropertyChanged();
            }
        }

        private async void EscHasErrorChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => EscHasError = value.Value.value).ConfigureAwait(true);
        }

        private bool _eScDisconnected;

        public bool EscDisconnected
        {
            get => _eScDisconnected;
            set
            {
                _eScDisconnected = value;
                OnPropertyChanged();
            }
        }

        private async void IsEscDisconnectedChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => EscDisconnected = value.Value.value).ConfigureAwait(true);
        }

        private string _fCBatteryThresholdBehavior;

        public string FcBatteryThresholdBehavior
        {
            get => _fCBatteryThresholdBehavior;
            set
            {
                _fCBatteryThresholdBehavior = value;
                OnPropertyChanged();
            }
        }


        private async void BatteryThresholdBehaviorChanged(object sender, FCBatteryThresholdBehaviorMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FcBatteryThresholdBehavior = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _seriousLowBatteryWarning;

        public bool SeriousLowBatteryWarning
        {
            get => _seriousLowBatteryWarning;
            set
            {
                _seriousLowBatteryWarning = value;
                OnPropertyChanged();
            }
        }

        private async void IsSeriousLowBatteryWarningChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SeriousLowBatteryWarning = value.Value.value).ConfigureAwait(true);
        }

        private bool _lowBatteryWarning;

        public bool LowBatteryWarning
        {
            get => _lowBatteryWarning;
            set
            {
                _lowBatteryWarning = value;
                OnPropertyChanged();
            }
        }

        private async void IsLowBatteryWarningChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => LowBatteryWarning = value.Value.value).ConfigureAwait(true);
        }

        private int _seriousLowBatteryWarningThreshold;

        public int SeriousLowBatteryWarningThreshold
        {
            get => _seriousLowBatteryWarningThreshold;
            set
            {
                _seriousLowBatteryWarningThreshold = value;
                OnPropertyChanged();
            }
        }

        private async void SeriousLowBatteryWarningThresholdChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SeriousLowBatteryWarningThreshold = value.Value.value).ConfigureAwait(true);
        }

        private int _lowBatteryWarningThreshold;

        public int LowBatteryWarningThreshold
        {
            get => _lowBatteryWarningThreshold;
            set
            {
                _lowBatteryWarningThreshold = value;
                OnPropertyChanged();
            }
        }

        private async void LowBatteryWarningThresholdChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => LowBatteryWarningThreshold = value.Value.value).ConfigureAwait(true);
        }

        private bool _requireLandingBySmartBatteryPercent;

        public bool RequireLandingBySmartBatteryPercent
        {
            get => _requireLandingBySmartBatteryPercent;
            set
            {
                _requireLandingBySmartBatteryPercent = value;
                OnPropertyChanged();
            }
        }

        private async void RequireLandingBySmartBatteryPercentChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RequireLandingBySmartBatteryPercent = value.Value.value).ConfigureAwait(true);
        }

        private bool _requireLandingByLowBatteryVoltage;

        public bool RequireLandingByLowBatteryVoltage
        {
            get => _requireLandingByLowBatteryVoltage;
            set
            {
                _requireLandingByLowBatteryVoltage = value;
                OnPropertyChanged();
            }
        }

        private async void RequireLandingByLowBatteryVoltageChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RequireLandingByLowBatteryVoltage = value.Value.value).ConfigureAwait(true);
        }

        private int _batteryPercentNeededToLand;

        public int BatteryPercentNeededToLand
        {
            get => _batteryPercentNeededToLand;
            set
            {
                _batteryPercentNeededToLand = value;
                OnPropertyChanged();
            }
        }

        private async void BatteryPercentNeededToLandChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => BatteryPercentNeededToLand = value.Value.value).ConfigureAwait(true);
        }

        private int _timeNeededToLand;

        public int TimeNeededToLand
        {
            get => _timeNeededToLand;
            set
            {
                _timeNeededToLand = value;
                OnPropertyChanged();
            }
        }

        private async void TimeNeededToLandChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => TimeNeededToLand = value.Value.value).ConfigureAwait(true);
        }

        private bool _isHomeLocationSet;

        public bool IsHomeLocationSet
        {
            get => _isHomeLocationSet;
            set
            {
                _isHomeLocationSet = value;
                OnPropertyChanged();
            }
        }

        private async void IsHomeLocationSetChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsHomeLocationSet = value.Value.value).ConfigureAwait(true);
        }

        private string _autoRthReason;

        public string AutoRthReason
        {
            get => _autoRthReason;
            set
            {
                _autoRthReason = value;
                OnPropertyChanged();
            }
        }

        private async void AutoRthReasonChanged(object sender, FCAutoRTHReasonMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => AutoRthReason = value.Value.value.ToString()).ConfigureAwait(true);
        }


        public double HomeLocationLatitude => HomeLocation.latitude;
        public double HomeLocationLongitude => HomeLocation.longitude;

        private LocationCoordinate2D _homeLocation;

        public LocationCoordinate2D HomeLocation
        {
            get => _homeLocation;
            set
            {
                _homeLocation = value;
                OnPropertyChanged($"HomeLocationLatitude");
                OnPropertyChanged($"HomeLocationLongitude");
            }
        }

        private async void HomeLocationChanged(object sender, LocationCoordinate2D? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => HomeLocation = value.Value).ConfigureAwait(true);
        }

        private bool _requireGoHomeBySmartBatteryPercent;

        public bool RequireGoHomeBySmartBatteryPercent
        {
            get => _requireGoHomeBySmartBatteryPercent;
            set
            {
                _requireGoHomeBySmartBatteryPercent = value;
                OnPropertyChanged();
            }
        }

        private async void RequireGoHomeBySmartBatteryPercentChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RequireGoHomeBySmartBatteryPercent = value.Value.value).ConfigureAwait(true);
        }

        private bool _requireGoHomeByLowBatteryVoltage;

        public bool RequireGoHomeByLowBatteryVoltage
        {
            get => _requireGoHomeByLowBatteryVoltage;
            set
            {
                _requireGoHomeByLowBatteryVoltage = value;
                OnPropertyChanged();
            }
        }

        private async void RequireGoHomeByLowBatteryVoltageChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RequireGoHomeByLowBatteryVoltage = value.Value.value).ConfigureAwait(true);
        }

        private int _smartRthCountdown;

        public int SmartRthCountdown
        {
            get => _smartRthCountdown;
            set
            {
                _smartRthCountdown = value;
                OnPropertyChanged();
            }
        }

        private async void SmartRthCountdownChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SmartRthCountdown = value.Value.value).ConfigureAwait(true);
        }

        private bool _shouldGoHome;

        public bool ShouldGoHome
        {
            get => _shouldGoHome;
            set
            {
                _shouldGoHome = value;
                OnPropertyChanged();
            }
        }

        private async void ShouldGoHomeChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => ShouldGoHome = value.Value.value).ConfigureAwait(true);
        }

        private int _batteryPercentNeededToGoHome;

        public int BatteryPercentNeededToGoHome
        {
            get => _batteryPercentNeededToGoHome;
            set
            {
                _batteryPercentNeededToGoHome = value;
                OnPropertyChanged();
            }
        }

        private async void BatteryPercentNeededToGoHomeChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => BatteryPercentNeededToGoHome = value.Value.value).ConfigureAwait(true);
        }

        private int _timeNeededToGoHome;

        public int TimeNeededToGoHome
        {
            get => _timeNeededToGoHome;
            set
            {
                _timeNeededToGoHome = value;
                OnPropertyChanged();
            }
        }

        private async void TimeNeededToGoHomeChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => TimeNeededToGoHome = value.Value.value).ConfigureAwait(true);
        }

        private int _distanceLimit;

        public int DistanceLimit
        {
            get => _distanceLimit;
            set
            {
                _distanceLimit = value;
                OnPropertyChanged();
            }
        }

        private async void DistanceLimitChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => DistanceLimit = value.Value.value).ConfigureAwait(true);
        }

        private int _hightLimit;

        public int HeightLimit
        {
            get => _hightLimit;
            set
            {
                _hightLimit = value;
                OnPropertyChanged();
            }
        }

        private async void HeightLimitChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => HeightLimit = value.Value.value).ConfigureAwait(true);
        }

        private string _fCRemoteControllerSwitchMode;

        public string FcRemoteControllerSwitchMode
        {
            get => _fCRemoteControllerSwitchMode;
            set
            {
                _fCRemoteControllerSwitchMode = value;
                OnPropertyChanged();
            }
        }

        private async void FcRemoteControllerSwitchModeChanged(object sender, FCRemoteControllerSwitchModeMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FcRemoteControllerSwitchMode = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private int _goHomeHeight;

        public int GoHomeHeight
        {
            get => _goHomeHeight;
            set
            {
                _goHomeHeight = value;
                OnPropertyChanged();
            }
        }

        private async void GoHomeHeightChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => GoHomeHeight = value.Value.value).ConfigureAwait(true);
        }

        private bool _isVisionSensorUsed;

        public bool IsVisionSensorUsed
        {
            get => _isVisionSensorUsed;
            set
            {
                _isVisionSensorUsed = value;
                OnPropertyChanged();
            }
        }

        private async void IsVisionSensorUsedChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsVisionSensorUsed = value.Value.value).ConfigureAwait(true);
        }

        private string _failSafeAction;

        public string FailSafeAction
        {
            get => _failSafeAction;
            set
            {
                _failSafeAction = value;
                OnPropertyChanged();
            }
        }

        private async void FailSafeActionChanged(object sender, FCFailsafeActionMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FailSafeAction = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _isLandingConfirmationNeeded;

        public bool IsLandingConfirmationNeeded
        {
            get => _isLandingConfirmationNeeded;
            set
            {
                _isLandingConfirmationNeeded = value;
                OnPropertyChanged();
            }
        }

        private async void IsLandingConfirmationNeededChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsLandingConfirmationNeeded = value.Value.value).ConfigureAwait(true);
        }

        private bool _isNearDistanceLimit;

        public bool IsNearDistanceLimit
        {
            get => _isNearDistanceLimit;
            set
            {
                _isNearDistanceLimit = value;
                OnPropertyChanged();
            }
        }

        private async void IsNearDistanceLimitChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsNearDistanceLimit = value.Value.value).ConfigureAwait(true);
        }

        private bool _isNearHeightLimit;

        public bool IsNearHeightLimit
        {
            get => _isNearHeightLimit;
            set
            {
                _isNearHeightLimit = value;
                OnPropertyChanged();
            }
        }

        private async void IsNearHeightLimitChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsNearHeightLimit = value.Value.value).ConfigureAwait(true);
        }

        private bool _groundStationModeEnabled;

        public bool GroundStationModeEnabled
        {
            get => _groundStationModeEnabled;
            set
            {
                _groundStationModeEnabled = value;
                OnPropertyChanged();
            }
        }

        private async void GroundStationModeEnabledChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => GroundStationModeEnabled = value.Value.value).ConfigureAwait(true);
        }

        private double _maxRadiusCanFlyAndGoHome;

        public double MaxRadiusCanFlyAndGoHome
        {
            get => _maxRadiusCanFlyAndGoHome;
            set
            {
                _maxRadiusCanFlyAndGoHome = value;
                OnPropertyChanged();
            }
        }

        private async void MaxRadiusCanFlyAndGoHomeChanged(object sender, DoubleMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => MaxRadiusCanFlyAndGoHome = value.Value.value).ConfigureAwait(true);
        }

        private string _aircraftName;

        public string AircraftName
        {
            get => _aircraftName;
            set
            {
                _aircraftName = value;
                OnPropertyChanged();
            }
        }

        private async void AircraftNameChanged(object sender, StringMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => AircraftName = value.Value.value).ConfigureAwait(true);
        }

        private string _compassCalibrationState;

        public string CompassCalibrationState
        {
            get => _compassCalibrationState;
            set
            {
                _compassCalibrationState = value;
                OnPropertyChanged();
            }
        }

        private async void CompassCalibrationStateChanged(object sender, FCCompassCalibrationStateMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => CompassCalibrationState = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private int _flightTimeInSeconds;

        public int FlightTimeInSeconds
        {
            get => _flightTimeInSeconds;
            set
            {
                _flightTimeInSeconds = value;
                OnPropertyChanged();
            }
        }

        private async void FlightTimeInSecondsChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FlightTimeInSeconds = value.Value.value).ConfigureAwait(true);
        }

        private bool _isCompassCalibrating;

        public bool IsCompassCalibrating
        {
            get => _isCompassCalibrating;
            set
            {
                _isCompassCalibrating = value;
                OnPropertyChanged();
            }
        }

        private async void IsCompassCalibratingChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsCompassCalibrating = value.Value.value).ConfigureAwait(true);
        }

        private bool _isSimulatorStarted;

        public bool IsSimulatorStarted
        {
            get => _isSimulatorStarted;
            set
            {
                _isSimulatorStarted = value;
                OnPropertyChanged();
            }
        }

        private async void IsSimulatorStartedChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsSimulatorStarted = value.Value.value).ConfigureAwait(true);
        }

        private double _takeoffLocationAltitude;

        public double TakeoffLocationAltitude
        {
            get => _takeoffLocationAltitude;
            set
            {
                _takeoffLocationAltitude = value;
                OnPropertyChanged();
            }
        }

        private async void TakeoffLocationAltitudeChanged(object sender, DoubleMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => TakeoffLocationAltitude = value.Value.value).ConfigureAwait(true);
        }

        private bool _noviceModeEnabled;

        public bool NoviceModeEnabled
        {
            get => _noviceModeEnabled;
            set
            {
                _noviceModeEnabled = value;
                OnPropertyChanged();
            }
        }

        private async void NoviceModeEnabledChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => NoviceModeEnabled = value.Value.value).ConfigureAwait(true);
        }

        private bool _isMotorStuck;

        public bool IsMotorStuck
        {
            get => _isMotorStuck;
            set
            {
                _isMotorStuck = value;
                OnPropertyChanged();
            }
        }

        private async void IsMotorStuckChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsMotorStuck = value.Value.value).ConfigureAwait(true);
        }

        private string _windWarning;

        public string WindWarning
        {
            get => _windWarning;
            set
            {
                _windWarning = value;
                OnPropertyChanged();
            }
        }

        private async void WindWarningChanged(object sender, FCWindWarningMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => WindWarning = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _hasIceOnPropellers;

        public bool HasIceOnPropellers
        {
            get => _hasIceOnPropellers;
            set
            {
                _hasIceOnPropellers = value;
                OnPropertyChanged();
            }
        }

        private async void HasIceOnPropellersChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => HasIceOnPropellers = value.Value.value).ConfigureAwait(true);
        }

        private bool _compassInstallError;

        public bool CompassInstallError
        {
            get => _compassInstallError;
            set
            {
                _compassInstallError = value;
                OnPropertyChanged();
            }
        }

        private async void CompassInstallErrorChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => CompassInstallError = value.Value.value).ConfigureAwait(true);
        }

        private string _motorStartFailureError;

        public string MotorStartFailureError
        {
            get => _motorStartFailureError;
            set
            {
                _motorStartFailureError = value;
                OnPropertyChanged();
            }
        }

        private async void MotorStartFailureErrorChanged(object sender, FCMotorStartFailureErrorMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => MotorStartFailureError = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _noPropellerInstalled;

        public bool NoPropellerInstalled
        {
            get => _noPropellerInstalled;
            set
            {
                _noPropellerInstalled = value;
                OnPropertyChanged();
            }
        }

        private async void NoPropellerInstalledChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => NoPropellerInstalled = value.Value.value).ConfigureAwait(true);
        }

        private string _gPsModeFailureReason;

        public string GpsModeFailureReason
        {
            get => _gPsModeFailureReason;
            set
            {
                _gPsModeFailureReason = value;
                OnPropertyChanged();
            }
        }

        private async void GpsModeFailureReasonChanged(object sender, FCGPSModeFailureReasonMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => GpsModeFailureReason = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _hasNoEnoughForce;

        public bool HasNotEnoughForce
        {
            get => _hasNoEnoughForce;
            set
            {
                _hasNoEnoughForce = value;
                OnPropertyChanged();
            }
        }

        private async void HasNotEnoughForceChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => HasNotEnoughForce = value.Value.value).ConfigureAwait(true);
        }

        private bool _compassHasError;

        public bool CompassHasError
        {
            get => _compassHasError;
            set
            {
                _compassHasError = value;
                OnPropertyChanged();
            }
        }

        private async void CompassHasErrorChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => CompassHasError = value.Value.value).ConfigureAwait(true);
        }

        private bool _isFlying;

        public bool IsFlying
        {
            get => _isFlying;
            set
            {
                _isFlying = value;
                OnPropertyChanged();
            }
        }

        private async void IsFlyingChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsFlying = value.Value.value).ConfigureAwait(true);
        }

        private bool _areMotorsOn;

        public bool AreMotorsOn
        {
            get => _areMotorsOn;
            set
            {
                _areMotorsOn = value;
                OnPropertyChanged();
            }
        }

        private async void AreMotorsOnChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => AreMotorsOn = value.Value.value).ConfigureAwait(true);
        }

        private int _satelliteCount;

        public int SatelliteCount
        {
            get => _satelliteCount;
            set
            {
                _satelliteCount = value;
                OnPropertyChanged();
            }
        }

        private async void SatelliteCountChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SatelliteCount = value.Value.value).ConfigureAwait(true);
        }

        private string _gPsSignalLevel;

        public string GpsSignalLevel
        {
            get => _gPsSignalLevel;
            set
            {
                _gPsSignalLevel = value;
                OnPropertyChanged();
            }
        }

        private async void GpsSignalLevelChanged(object sender, FCGPSSignalLevelMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => GpsSignalLevel = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private bool _isImuWarmingUp;

        public bool IsImuWarmingUp
        {
            get => _isImuWarmingUp;
            set
            {
                _isImuWarmingUp = value;
                OnPropertyChanged();
            }
        }

        private async void IsImuWarmingUpChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => IsImuWarmingUp = value.Value.value).ConfigureAwait(true);
        }

        private string _flightMode;

        public string FlightMode
        {
            get => _flightMode;
            set
            {
                _flightMode = value;
                OnPropertyChanged();
            }
        }

        private async void FlightModeChanged(object sender, FCFlightModeMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FlightMode = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private int _remainingFlightTime;

        public int RemainingFlightTime
        {
            get => _remainingFlightTime;
            set
            {
                _remainingFlightTime = value;
                OnPropertyChanged();
            }
        }

        private async void RemainingFlightTimeChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => RemainingFlightTime = value.Value.value).ConfigureAwait(true);
        }

        public double AttitudePitch => Attitude.pitch;
        public double AttitudeRoll => Attitude.roll;
        public double AttitudeYaw => Attitude.yaw;

        private Attitude _attitude;

        public Attitude Attitude
        {
            get => _attitude;
            set
            {
                _attitude = value;
                OnPropertyChanged($"AttitudePitch");
                OnPropertyChanged($"AttitudeRoll");
                OnPropertyChanged($"AttitudeYaw");
            }
        }

        private async void AttitudeChanged(object sender, Attitude? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Attitude = value.Value).ConfigureAwait(true);
        }

        private double _altitude;

        public double Altitude
        {
            get => _altitude;
            set
            {
                _altitude = value;
                OnPropertyChanged();
            }
        }

        private async void AltitudeChanged(object sender, DoubleMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Altitude = value.Value.value).ConfigureAwait(true);
        }

        public double VelocityX => Velocity3D.x;
        public double VelocityY => Velocity3D.y;
        public double VelocityZ => Velocity3D.z;

        private Velocity3D _velocity3D;

        public Velocity3D Velocity3D
        {
            get => _velocity3D;
            set
            {
                _velocity3D = value;
                OnPropertyChanged($"VelocityX");
                OnPropertyChanged($"VelocityY");
                OnPropertyChanged($"VelocityZ");
            }
        }

        private async void VelocityChanged(object sender, Velocity3D? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Velocity3D = value.Value).ConfigureAwait(true);
        }

        public double AircraftLocationLatitude => AircraftLocation.latitude;
        public double AircraftLocationLongitude => AircraftLocation.longitude;

        private LocationCoordinate2D _aircraftLocation;

        public LocationCoordinate2D AircraftLocation
        {
            get => _aircraftLocation;
            set
            {
                _aircraftLocation = value;
                OnPropertyChanged($"AircraftLocationLatitude");
                OnPropertyChanged($"AircraftLocationLongitude");
            }
        }

        private async void AircraftLocationChanged(object sender, LocationCoordinate2D? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => AircraftLocation = value.Value).ConfigureAwait(true);
        }

        private string _startCompassCalibrationStatus;

        public string StartCompassCalibrationStatus
        {
            get => _startCompassCalibrationStatus;
            set
            {
                _startCompassCalibrationStatus = value;
                OnPropertyChanged();
            }
        }


        public ICommand StartCompassCalibrationCommand;
        public ICommand StartCompassCalibration
        {
            get
            {
                return StartCompassCalibrationCommand ?? (StartCompassCalibrationCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartCompasCalibrationAsync()
                        .ConfigureAwait(true);
                    StartCompassCalibrationStatus = res == SDKError.NO_ERROR ? "Start Calibration Successfully" : res.ToString();
                }, () => true));
            }
        }

        private string _stopCompassCalibrationStatus;

        public string StopCompassCalibrationStatus
        {
            get => _stopCompassCalibrationStatus;
            set
            {
                _stopCompassCalibrationStatus = value;
                OnPropertyChanged();
            }
        }


        public ICommand StopCompassCalibrationCommand;
        public ICommand StopCompassCalibration
        {
            get
            {
                return StopCompassCalibrationCommand ?? (StopCompassCalibrationCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StopCompassCalibrationAsync()
                        .ConfigureAwait(true);
                    StopCompassCalibrationStatus = res == SDKError.NO_ERROR ? "Stop Calibration Successfully" : res.ToString();
                }, () => true));
            }
        }

        private string _startGoHomeStatus;

        public string StartGoHomeStatus
        {
            get => _startGoHomeStatus;
            set
            {
                _startGoHomeStatus = value;
                OnPropertyChanged();
            }
        }


        public ICommand StartGoHomeCommand;
        public ICommand StartGoHome
        {
            get
            {
                return StartGoHomeCommand ?? (StartGoHomeCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartGoHomeAsync()
                        .ConfigureAwait(true);
                    StartGoHomeStatus = res == SDKError.NO_ERROR ? "Go Home started Successfully" : res.ToString();
                }, () => true));
            }
        }

        private string _stopGoHomeStatus;

        public string StopGoHomeStatus
        {
            get => _stopGoHomeStatus;
            set
            {
                _stopGoHomeStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand StopGoHomeCommand;
        public ICommand StopGoHome
        {
            get
            {
                return StopGoHomeCommand ?? (StopGoHomeCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StopGoHomeAsync()
                        .ConfigureAwait(true);
                    StopGoHomeStatus = res == SDKError.NO_ERROR ? "Go Home stopped Successfully" : res.ToString();
                }, () => true));
            }
        }

        private string _getAutoRthReason;

        public string GetAutoRthReasonResult
        {
            get => _getAutoRthReason;
            set
            {
                _getAutoRthReason = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetAutoRthReasonCommand;
        public ICommand GetAutoRthReason
        {
            get
            {
                return GetAutoRthReasonCommand ?? (GetAutoRthReasonCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetAutoRTHReasonAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        GetAutoRthReasonResult = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        GetAutoRthReasonResult = res.error.ToString();
                    }
                }, () => true));
            }
        }


        private string _getAircraftLocationError;

        public string GetAircraftLocationError
        {
            get => _getAircraftLocationError;
            set
            {
                _getAircraftLocationError = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetAircraftLocationCommand;
        public ICommand GetAircraftLocation
        {
            get
            {
                return GetAircraftLocationCommand ?? (GetAircraftLocationCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetAircraftLocationAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        AircraftLocation = res.value.Value; //result
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        GetAircraftLocationError = res.error.ToString(); //only for error
                    }
                }, () => true));
            }
        }

        public ICommand GetFlightModeCommand;
        public ICommand GetFlightMode
        {
            get
            {
                return GetFlightModeCommand ?? (GetFlightModeCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetFlightModeAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        FlightMode = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        FlightMode = res.error.ToString();
                    }
                }, () => true));
            }
        }

        public ICommand GetFcRemoteControllerSwitchModeCommand;
        public ICommand GetFcRemoteControllerSwitchMode
        {
            get
            {
                return GetFcRemoteControllerSwitchModeCommand ?? (GetFcRemoteControllerSwitchModeCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetFCRemoteControllerSwitchModeAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        FcRemoteControllerSwitchMode = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        FcRemoteControllerSwitchMode = res.error.ToString();
                    }
                }, () => true));
            }
        }

        private string _autoLandStatus;

        public string AutoLandStatus
        {
            get => _autoLandStatus;
            set
            {
                _autoLandStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        private string _autoLandStopStatus;

        public string AutoLandStopStatus
        {
            get => _autoLandStopStatus;
            set
            {
                _autoLandStopStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }

        public ICommand StartAutoLandingCommand;
        public ICommand StartAutoLanding
        {
            get
            {
                return StartAutoLandingCommand ?? (StartAutoLandingCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                        .StartAutoLandingAsync().ConfigureAwait(true);
                    AutoLandStatus = res.ToString();
                }, () => true));
            }
        }

        public ICommand StopAutoLandingCommand;
        public ICommand StopAutoLanding
        {
            get
            {
                return StopAutoLandingCommand ?? (StopAutoLandingCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                        .StopAutoLandingAsync().ConfigureAwait(true);
                    AutoLandStopStatus = res.ToString();
                }, () => true));
            }
        }

        private string _takeOffStatus;

        public string TakeOffStatus
        {
            get { return _takeOffStatus; }
            set
            {
                _takeOffStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }


        public ICommand StartTakeoffCommand;
        public ICommand StartTakeoff
        {
            get
            {
                return StartTakeoffCommand ?? (StartTakeoffCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                        .StartTakeoffAsync().ConfigureAwait(true);
                    TakeOffStatus = res.ToString();
                }, () => true));
            }
        }

        public string EscBeepEnabled { get; set; }

        private string _escBeepChangeStatus;

        public string EscBeepChangeStatus
        {
            get { return _escBeepChangeStatus; }
            set
            {
                _escBeepChangeStatus = value == SDKError.NO_ERROR.ToString() ? "Success" : value;
                OnPropertyChanged();
            }
        }


        public ICommand SetEscCommand;
        public ICommand SetEsc
        {
            get
            {
                return SetEscCommand ?? (SetEscCommand = new RelayCommand(async delegate
                {
                    if (!bool.TryParse(EscBeepEnabled, out bool result))
                    {
                        EscBeepChangeStatus = "Failed parsing value";
                        return;
                    }
                    var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0)
                        .SetESCBeepEnabledAsync(new BoolMsg { value = result }).ConfigureAwait(true);
                    EscBeepChangeStatus = res.ToString();
                }, () => true));
            }
        }

        private async Task OnUiThreadAsync(Action action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await _dispatcher.RunAsync(priority, () => action());
        }
    }
}
