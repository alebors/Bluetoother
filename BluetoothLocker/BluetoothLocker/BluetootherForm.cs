using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BluetoothLocker
{
    public partial class BluetootherForm : Form
    {
        #region Private members

        private readonly string _aesRandomKey = AesCipher.GenerateRandomKey();
        private string _userNameEncrypted;
        private string _userPassEncrypted;
        private bool _secured = false;
        private DevicesConnector _deviceConnector = new DevicesConnector(); //will be used for devices discovering, checking their presence
        private bool _isLocked = false;

        #endregion

        #region Constructor

        public BluetootherForm()
        {
            InitializeComponent();
            _deviceConnector.DiscoverComplete += deviceConnector_DiscoverComplete;
            _deviceConnector.DeviceWentOutOfRange += deviceConnector_DeviceWentOutOfRange;
        }

        #endregion

        #region Methods

        private void StartSecurity()
        {
            devicesDropDown.Enabled = false;
            discoverBtn.Enabled = false;

            startStopBtn.Text = Messages.StopBtnText;

            _deviceConnector.StartDeviceMonitoring(SelectedDevice);
            
            _secured = true;
        }

        private void StopSecurity()
        {
            devicesDropDown.Enabled = true;
            discoverBtn.Enabled = true;

            _deviceConnector.StopDeviceMonitoring();

            startStopBtn.Text = Messages.StartBtnText;
            _secured = false;
        }

        private BluetoothDeviceInfo SelectedDevice
        {
            get
            {
                var selectedDeviceItem = devicesDropDown.SelectedItem as DeviceItem;
                return selectedDeviceItem.Info;
            }
        }

        private void LockTheProgram()
        {
            if (_isLocked)
                return;

            _deviceConnector.StopDeviceMonitoring();
            var cf = new CredentialsForm(ValidateCredentials);
            _isLocked = true;
            cf.ShowDialog(); // no need to check result of dialog because it can be only OK or nothing
            _isLocked = false;
            _deviceConnector.StartDeviceMonitoring(SelectedDevice); // start checking again because user can go away again just after unlocking the program
        }

        private bool ValidateCredentials(string name, string password)
        {
            return AesCipher.EncryptText(name, _aesRandomKey) == _userNameEncrypted && AesCipher.EncryptText(password, _aesRandomKey) == _userPassEncrypted && DevicesConnector.IsDeviceInRange(SelectedDevice);
        }

        #endregion

        #region Event Handlers

        private void deviceConnector_DeviceWentOutOfRange(BluetoothDeviceInfo obj)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate { LockTheProgram(); });
            }
            else
            {
                LockTheProgram();
            }
        }

        private void deviceConnector_DiscoverComplete(IList<DeviceItem> deviceList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate { deviceConnector_DiscoverComplete(deviceList); });
            }
            else
            {
                devicesDropDown.DataSource = deviceList;
                if (devicesDropDown.Items.Count != 0)
                    devicesDropDown.SelectedIndex = 0;

                devicesDropDown.Enabled = true;
                discoverBtn.Enabled = true;
                startStopBtn.Enabled = true;
            }
        }

        private void discoverBtn_Click(object sender, EventArgs e)
        {
            discoverBtn.Enabled = false;
            devicesDropDown.Enabled = false;
            startStopBtn.Enabled = false;

            _deviceConnector.StartDevicesDiscover();
        }

        private void startStopBtn_Click(object sender, EventArgs e)
        {
            if (!_secured)
                StartSecurity();
            else
                StopSecurity();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BluetootherForm_Shown(object sender, EventArgs e)
        {
            var cf = new CredentialsForm(true);
            var result = cf.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                Close();
                return;
            }

            _userNameEncrypted = AesCipher.EncryptText(cf.UserName, _aesRandomKey);
            _userPassEncrypted = AesCipher.EncryptText(cf.UserPassword, _aesRandomKey);
        }

        private void devicesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            startStopBtn.Enabled = devicesDropDown.SelectedItem != null;
        }

        #endregion
    }
}
