using System.Threading.Tasks;
using DJI.WindowsSDK;
using DJIUWPSample.ViewModels;

namespace DJIWindowsSDKSample.ViewModels
{
    public class VirtualRemoteControllerViewModel : ViewModelBase
    {
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
    }
}
