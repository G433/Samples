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
    public class BatteryViewModel : ViewModelBase
    {
        private readonly CoreDispatcher _dispatcher;

        public BatteryViewModel()
        {
            _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).FullChargeCapacityChanged += FullChargeCapacityChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).ChargeRemainingChanged += ChargeRemainingChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).ChargeRemainingInPercentChanged += ChargeRemainingInPercentChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).VoltageChanged += VoltageChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).CurrentChanged += CurrentChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).BatteryTemperatureChanged += BatteryTemperatureChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).ConnectionChanged += ConnectionChanged;
            DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).SerialNumberChanged += SerialNumberChanged;
        }

        private string _fullChargeCapacity;

        public string FullChargeCapacity
        {
            get => _fullChargeCapacity;
            set
            {
                _fullChargeCapacity = value;
                OnPropertyChanged();
            }
        }

        private int _chargeRemaining;

        public int ChargeRemaining
        {
            get => _chargeRemaining;
            set
            {
                _chargeRemaining = value;
                OnPropertyChanged();
            }
        }

        private string _chargeRemainingInPercent;

        public string ChargeRemainingInPercent
        {
            get => _chargeRemainingInPercent;
            set
            {
                _chargeRemainingInPercent = $"{value}%";
                OnPropertyChanged();
            }
        }

        private int _voltage;

        public int Voltage
        {
            get => _voltage;
            set
            {
                _voltage = value;
                OnPropertyChanged();
            }
        }

        private int _current;

        public int Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }

        private double _batteryTemperature;

        public double BatteryTemperature
        {
            get => _batteryTemperature;
            set
            {
                _batteryTemperature = value;
                OnPropertyChanged();
            }
        }

        private string _connection;

        public string Connection
        {
            get => _connection;
            set
            {
                _connection = value;
                OnPropertyChanged();
            }
        }

        private string _serialNumber;

        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        private async void SerialNumberChanged(object sender, StringMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => SerialNumber = value.Value.value).ConfigureAwait(true);
        }

        private async void ConnectionChanged(object sender, BoolMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Connection = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private async void BatteryTemperatureChanged(object sender, DoubleMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => BatteryTemperature = value.Value.value).ConfigureAwait(true);
        }

        private async void CurrentChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Current = value.Value.value).ConfigureAwait(true);
        }

        private async void VoltageChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => Voltage = value.Value.value).ConfigureAwait(true);
        }

        private async void ChargeRemainingInPercentChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => ChargeRemainingInPercent = value.Value.value.ToString()).ConfigureAwait(true);
        }

        private async void ChargeRemainingChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => ChargeRemaining = value.Value.value).ConfigureAwait(true);
        }

        private async void FullChargeCapacityChanged(object sender, IntMsg? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            await OnUiThreadAsync(() => FullChargeCapacity = value.Value.value.ToString()).ConfigureAwait(true);
        }

        public ICommand GetFullChargedCapacityCommand;
        public ICommand GetFullChargedCapacity
        {
            get
            {
                return GetFullChargedCapacityCommand ?? (GetFullChargedCapacityCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).GetFullChargeCapacityAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        FullChargeCapacity = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        FullChargeCapacity = res.error.ToString();
                    }
                }, () => true));
            }
        }

        public ICommand GetSerialNumberCommand;
        public ICommand GetSerialNumber
        {
            get
            {
                return GetSerialNumberCommand ?? (GetSerialNumberCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0)
                        .GetSerialNumberAsync().ConfigureAwait(true);
                    if (res.value != null && res.error == SDKError.NO_ERROR)
                    {
                        SerialNumber = res.value.Value.value;
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        SerialNumber = res.error.ToString();
                    }
                }, () => true));
            }
        }

        public ICommand GetConnectionCommand;
        public ICommand GetConnection
        {
            get
            {
                return GetConnectionCommand ?? (GetConnectionCommand = new RelayCommand(async delegate
                {
                    var res = await DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).GetConnectionAsync()
                        .ConfigureAwait(true);
                    if (res.error == SDKError.NO_ERROR && res.value != null)
                    {
                        Connection = res.value.Value.value.ToString();
                    }
                    else if (res.error != SDKError.NO_ERROR)
                    {
                        Connection = res.error.ToString();
                    }
                }, () => true));
            }
        }

        private async Task OnUiThreadAsync(Action action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await _dispatcher.RunAsync(priority, () => action());
        }
    }
}

