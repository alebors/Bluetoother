using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BluetoothLocker
{
    internal class Decryptor
    {
        private IPasswordStorage _pwdStorage;

        public Decryptor(IPasswordStorage pwdStorage)
        {
            _pwdStorage = pwdStorage;
        }

        public byte[] RSADecrypt(byte[] dataToDecrypt)
        {
            try
            {
                var rsaAlg = new RSACryptoServiceProvider();
                rsaAlg.ImportParameters(_pwdStorage.PrivateKey);
                return rsaAlg.Decrypt(dataToDecrypt, true);
            }
            catch (CryptographicException e)
            {
                Debug.Assert(false, e.ToString());
                return null;
            }

        }

        private byte[] Decrypt(byte[] encriptedBytes, byte[] baseKey, byte[] iv, byte[] salt)
        {
            byte[] decryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using (var aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(baseKey, salt, 500);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encriptedBytes, 0, encriptedBytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public string DecryptText(CipherMessage encryptedMsg)
        {
            var key = RSADecrypt(encryptedMsg.Key);
            var iv = RSADecrypt(encryptedMsg.IV);
            var salt = RSADecrypt(encryptedMsg.Salt);

            var bytesDecrypted = Decrypt(encryptedMsg.Data, key, iv, salt);
            
            return Encoding.UTF8.GetString(bytesDecrypted);
        }
    }
}
