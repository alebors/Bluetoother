using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLocker
{
    class WindowsKeyStorage : IPasswordStorage, IDisposable
    {
        private int _keyLength = 2048;
        private string _containerName;

        public WindowsKeyStorage(RSACryptoServiceProvider rSACryptoServiceProvider)
        {
            byte[] random = new Byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            _containerName = Convert.ToBase64String(random); // the string key for storing/retreiving private key form windows key storage

            PublicKey = rSACryptoServiceProvider.ExportParameters(false);
            PrivateKey = rSACryptoServiceProvider.ExportParameters(true);
        }
        
        public RSAParameters PublicKey { get; set; }  //public key was not requested to be stored in windows storage

        public RSAParameters PrivateKey 
        {
            get
            { 
                CspParameters cp = new CspParameters();  
                cp.KeyContainerName = _containerName;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_keyLength, cp);
                return rsa.ExportParameters(true);
            }
            set 
            {
                CspParameters cp = new CspParameters();  
                cp.KeyContainerName = _containerName;
                var rsa = new RSACryptoServiceProvider(_keyLength, cp);
                rsa.ImportParameters(value);
            } 
        }

        private void DeleteKeyFromContainer()
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = _containerName;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);
            rsa.PersistKeyInCsp = false;
            rsa.Clear();
        }

        public void Dispose()
        {
            DeleteKeyFromContainer();
        }
    }
}
