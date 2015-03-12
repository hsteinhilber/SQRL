using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using CryptSharp.Utility;

namespace SQRL.Security {
    public class PasswordHasher {
        public PasswordHasher(string password) {
            this.Password = password ?? String.Empty;
        }

        public string Password { get; private set; }

        //TODO: Add properties for Scrypt function parameters

        public byte[] EnScrypt(byte[] salt, int iterations) {
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations", "iterations must be greater than 0");

            var key = Password == "" ? new byte[] { } : 
                      CryptographicBuffer.ConvertStringToBinary(Password, BinaryStringEncoding.Utf8).ToArray();
            //salt = salt ?? new byte[] { };

            var result = salt = SCrypt.ComputeDerivedKey(key, salt, 512, 256, 1, null, 32);
            for (int i = 1; i < iterations; ++i) {
                salt = SCrypt.ComputeDerivedKey(key, salt, 512, 256, 1, null, 32);
                result.Xor(salt);
            }

            return result;
        }

        public byte[] EnScrypt(byte[] salt, TimeSpan duration, out int iterations) {
            if (duration <= TimeSpan.FromSeconds(0)) throw new ArgumentOutOfRangeException("duration", "duration must be greater than 0");

            iterations = 0;

            return null;
        }
    }
}
