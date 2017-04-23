using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLocker
{
    internal class CipherMessage
    {
        public CipherMessage(byte[] data, byte[] key, byte[] iv, byte[] salt)
        {
            Data = data;
            Key = key;
            IV = iv;
            Salt = salt;
        }

        public byte[] Data { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public byte[] Salt { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CipherMessage;
            if (other == null)
                return false;
            return Data.SequenceEqual(other.Data);
        }
        public override int GetHashCode()
        {
            // just to avoid warning because equals already overridden
            return base.GetHashCode();
        }
    }
}
