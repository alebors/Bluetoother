using System;
using System.Security.Cryptography;

namespace BluetoothLocker
{
    internal interface IPasswordStorage: IDisposable
    {
        RSAParameters PrivateKey { get; set; }
        RSAParameters PublicKey { get; set; }
    }
}
