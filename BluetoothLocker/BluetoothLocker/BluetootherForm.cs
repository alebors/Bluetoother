using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Security.Cryptography;

namespace BluetoothLocker
{
    public partial class BluetootherForm : Form
    {
        #region Private members

        private CipherMessage _userNameEncrypted;
        private CipherMessage _userPassEncrypted;
        private string _userPass;
        private bool _secured = false;
        private DevicesConnector _deviceConnector = new DevicesConnector(); //will be used for devices discovering, checking their presence
        private bool _isLocked = false;
        private Encryptor _encryptor;
        private Decryptor _decryptor;
        private IPasswordStorage _pwdStorage;

        #endregion

        #region Constructor

        public BluetootherForm()
        {
            InitializeComponent();
            _deviceConnector.DiscoverComplete += deviceConnector_DiscoverComplete;
            _deviceConnector.DeviceWentOutOfRange += deviceConnector_DeviceWentOutOfRange;

            //_pwdStorage = new LocalPasswordStorage(new RSACryptoServiceProvider(2048));
            _pwdStorage = new WindowsKeyStorage(new RSACryptoServiceProvider(2048));
            _encryptor = new Encryptor(_pwdStorage);
            _decryptor = new Decryptor(_pwdStorage);
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
            return _encryptor.EncryptText(name) == _userNameEncrypted && _encryptor.EncryptText(password) == _userPassEncrypted && DevicesConnector.IsDeviceInRange(SelectedDevice);
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

            _userPass = cf.UserPassword;
            _userNameEncrypted = _encryptor.EncryptText(cf.UserName);
            _userPassEncrypted = _encryptor.EncryptText(cf.UserPassword);
        }

        private void devicesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            startStopBtn.Enabled = devicesDropDown.SelectedItem != null;
        }

        #endregion

        private void showInfoBtn_Click(object sender, EventArgs e)
        {
            var f = new DisplayPasswords() { PassInfo = PrepareData() };
            f.ShowDialog();
        }

        private string PrepareData()
        {
            var s2 = string.Join(", ", _userPassEncrypted.Data.Select(b => b.ToString()).ToArray());
            return string.Format("Password: {0}\r\nEncryptedPass: {1}\r\nDecryptedPass: {2}", _userPass, s2, _decryptor.DecryptText(_userPassEncrypted));
        }

        private void BluetootherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_pwdStorage is IDisposable)
                (_pwdStorage as IDisposable).Dispose();
        }
    }
}
