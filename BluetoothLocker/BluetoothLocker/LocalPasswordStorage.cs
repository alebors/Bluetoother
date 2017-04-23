using System.Security.Cryptography;

namespace BluetoothLocker
{
    class LocalPasswordStorage : IPasswordStorage
    {
        public LocalPasswordStorage(RSACryptoServiceProvider rSACryptoServiceProvider)
        {
            PublicKey = rSACryptoServiceProvider.ExportParameters(false);
            PrivateKey = rSACryptoServiceProvider.ExportParameters(true);
        }

        public RSAParameters PrivateKey { get; set; }
        public RSAParameters PublicKey { get; set; }
    }
}
