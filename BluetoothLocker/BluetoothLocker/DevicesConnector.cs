using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace BluetoothLocker
{
    /// <summary>
    /// Asynchronously discovers devices in range, checks selected device presence
    /// </summary>
    internal class DevicesConnector
    {
        #region Private members

        private readonly Guid _fakeServiceId = Guid.NewGuid();
        private Timer _timer;
        private object _timerLocker = new object(); //since _timer object is accessed from timer thread and from client side thread(UI) we need to synchronize access to the object

        #endregion

        #region Events

        public Action<IList<DeviceItem>> DiscoverComplete;
        public Action<BluetoothDeviceInfo> DeviceWentOutOfRange;

        #endregion

        #region Methods
        
        /// <summary>
        /// Using ansynchronous discovering so that don't lock UI thread
        /// </summary>
        public void StartDevicesDiscover()
        {
            var btClient = new BluetoothClient();
            btClient.BeginDiscoverDevices(100, true, true, true, true, EndDevicesDiscover, btClient);
            Debug.Assert(DiscoverComplete != null, "Client must be subscribed to DiscoverComplete");
        }

        /// <summary>
        /// Callback for BeginDiscoverDevices
        /// </summary>
        /// <param name="ar"></param>
        private void EndDevicesDiscover(IAsyncResult ar)
        {
            var btClient = ar.AsyncState as BluetoothClient;
            var devices = btClient.EndDiscoverDevices(ar);

            //raising an event to subscribers
            DiscoverComplete(devices.Select(d => new DeviceItem(d)).ToList());
        }

        /// <summary>
        /// Synchronously checks if device is in range. Used to call from separate thread(timer) and during unlocking program by user
        /// </summary>
        /// <param name="device">Device info</param>
        /// <returns>True if device is in range</returns>
        public static bool IsDeviceInRange(BluetoothDeviceInfo device)
        {
            var fakeServiceId = Guid.NewGuid();
            try
            {
                //just try to access non-existing service. If not exceptiono thrown - device is in range
                device.GetServiceRecords(fakeServiceId);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        /// <summary>
        /// Starts a timer for checking device presence. Once found device is out of range it stops raises an event and stops monitoring automatically
        /// </summary>
        /// <param name="device">Device information</param>
        public void StartDeviceMonitoring(BluetoothDeviceInfo device)
        {
            lock (_timerLocker)
            {
                Debug.Assert(DeviceWentOutOfRange != null, "Client must be described to DeviceWentOutOfRange event");
                _timer = new Timer(OnTimerTick, device, 1000, 2000);
            }
        }

        /// <summary>
        /// Stops monitoring device. Can be stopped atutomatically or manually form client code
        /// </summary>
        public void StopDeviceMonitoring()
        {
            lock (_timerLocker)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        /// <summary>
        /// callback for timer. Called with specified in timer period.
        /// </summary>
        /// <param name="state">As a state object we pass BloetoothDeviceInfo so that we are able to check its presence</param>
        private void OnTimerTick(object state)
        {
            if (!IsDeviceInRange(state as BluetoothDeviceInfo))
            {
                StopDeviceMonitoring();
                DeviceWentOutOfRange(state as BluetoothDeviceInfo);
            }
        }

        #endregion
    }
}
