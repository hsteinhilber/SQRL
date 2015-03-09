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

            var result = new byte[32];

            for (int i = 0; i < iterations; ++i) {

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
            return salt;
        }

        public static IBuffer Xor(byte[] left, byte[] right) {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Length != right.Length) throw new ArgumentException("buffers must have the same length");

            var result = new byte[left.Length];
            for (int i = 0; i < left.Length; ++i) {
                result[i] = (byte)(left[i] ^ right[i]);
            }
            return result.AsBuffer();
        }
    }
}
