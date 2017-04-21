using InTheHand.Net.Sockets;
using System.Diagnostics;

namespace BluetoothLocker
{
    internal class DeviceItem
    {
        internal DeviceItem(BluetoothDeviceInfo info)
        {
            Debug.Assert(info != null, "BluetoothDeviceInfo must not be null");
            Info = info;
        }

        public BluetoothDeviceInfo Info
        {
            get;
            set;
        }

        /// <summary>
        /// This override is needed displaying device names in ComboBox
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Info.DeviceName;
        }
    }
}
