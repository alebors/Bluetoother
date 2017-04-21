using InTheHand.Net.Sockets;
using System.Diagnostics;

namespace BluetoothLocker
{
    /// <summary>
    /// Wrapper for BluetoothDeviceInfor for combobox control
    /// </summary>
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
        /// This override is needed for displaying device names in ComboBox
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Info.DeviceName;
        }
    }
}
