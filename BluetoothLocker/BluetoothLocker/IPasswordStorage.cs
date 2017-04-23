using System;
using System.Security.Cryptography;

namespace BluetoothLocker
{
    internal interface IPasswordStorage
    {
        RSAParameters PrivateKey { get; set; }
        RSAParameters PublicKey { get; set; }
    }
}
