using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SQRL
{
    public static class SecurityExtensions {
        public static byte[] EnScrypt(this string password, byte[] salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations", "iterations must be greater than 0");

            salt = salt ?? new byte[] { };
            var result = Scrypt(password, salt);

            for (int i = 0; i < iterations; ++i) {
                salt = Scrypt(password, salt);
                result.Xor(salt);
            }

            return result;
        }

        public static IBuffer EnScrypt(this string password, byte[] salt, TimeSpan duration, out int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (duration <= TimeSpan.FromSeconds(0)) throw new ArgumentOutOfRangeException("duration", "duration must be greater than 0");

            iterations = 0;

            return null;
        }

        private static byte[] Scrypt(string password, byte[] salt) {
            return CryptographicBuffer.GenerateRandom(32).ToArray();
        }

        public static byte[] Xor(this byte[] left, byte[] right) {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Length != right.Length) throw new ArgumentException("buffers must have the same length");

            for (int i = 0; i < left.Length; ++i) {
                left[i] ^= right[i];
            }
            return left;
        }
    }
}
