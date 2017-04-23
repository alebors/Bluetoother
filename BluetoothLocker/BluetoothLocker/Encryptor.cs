using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BluetoothLocker
{
    internal class Encryptor
    {
        private IPasswordStorage _pwdStorage;

        public Encryptor(IPasswordStorage pwdStorage)
        {
            _pwdStorage = pwdStorage;
        }

        private byte[] RSAEncrypt(byte[] dataToEncrypt)
        {
            try
            {
                RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider();
                rsaAlg.ImportParameters(_pwdStorage.PublicKey);
                return rsaAlg.Encrypt(dataToEncrypt, true);
            }
            catch (CryptographicException e)
            {
                Debug.Assert(false, e.ToString());
                return null;
            }
        }

        private byte[] GenerateRandomBytes(int size)
        {
            byte[] random = new Byte[size];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            return random;
        }

        public byte[] Encrypt(byte[] dataBytes, byte[] basekey, byte[] iv, byte[] salt)
        {
            byte[] encryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using (var aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(basekey, salt, 500);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = iv;

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataBytes, 0, dataBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public CipherMessage EncryptText(string input)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);

            var aesKey = GenerateRandomBytes(32);
            var aesIV = GenerateRandomBytes(16);
            var aesSolt = GenerateRandomBytes(32);
            var bytesEncrypted = Encrypt(bytesToBeEncrypted, aesKey, aesIV, aesSolt);

            return new CipherMessage(bytesEncrypted, RSAEncrypt(aesKey), RSAEncrypt(aesIV), RSAEncrypt(aesSolt));
        }
    }
}
