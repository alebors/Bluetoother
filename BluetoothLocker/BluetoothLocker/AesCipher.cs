using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BluetoothLocker
{
    internal static class AesCipher
    {
        private static readonly byte[] _saltBytes = new byte[] { 5, 4, 3, 0, 1, 10, 32, 18 };

        public static string GenerateRandomKey()
        {
            return Guid.NewGuid().ToString();
        }

        private static byte[] Encrypt(byte[] dataBytes, byte[] pwdBytes)
        {
            byte[] encryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using (var aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(pwdBytes, _saltBytes, 500);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

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
        
        public static string EncryptText(string input, string password)
        {
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }
    }
}
