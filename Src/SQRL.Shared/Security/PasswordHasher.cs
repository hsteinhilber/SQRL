using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SQRL.Security {
    public class PasswordHasher {
        public PasswordHasher(string password) {
            this.Password = password ?? String.Empty;
        }

        public string Password { get; private set; }

        //TODO: Add properties for Scrypt function parameters

        public byte[] EnScrypt(byte[] salt, int iterations) {
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations", "iterations must be greater than 0");

            salt = salt ?? new byte[] { };
            var result = Scrypt(salt, 1, 512, 256, 32);

            for (int i = 0; i < iterations; ++i) {
                salt = Scrypt(salt, 1, 512, 256, 32);
                result.Xor(salt);
            }

            return result;
        }

        public byte[] EnScrypt(byte[] salt, TimeSpan duration, out int iterations) {
            if (duration <= TimeSpan.FromSeconds(0)) throw new ArgumentOutOfRangeException("duration", "duration must be greater than 0");

            iterations = 0;

            return null;
        }

        private byte[] Scrypt(byte[] salt, int p, int N, int r, int size) {
            return CryptographicBuffer.GenerateRandom(32).ToArray();
        }
    }
}
