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
        public static IBuffer EnScrypt(this string password, IBuffer salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations", "iterations must be greater than 0");

            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesGcm);

            for (int i = 0; i < iterations; ++i) {

            }
            var pwdBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8) ??
                            CryptographicBuffer.CreateFromByteArray(new byte[] { });
            salt = salt ?? CryptographicBuffer.DecodeFromHexString("a8ea62a6e1bfd20e4275011595307aa302645c1801600ef5cd79bf9d884d911c"); // THIS IS WRONG!!!!
            return salt; // THIS IS WRONG TOO
        }

        public static IBuffer EnScrypt(this string password, IBuffer salt, TimeSpan duration, out int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (duration <= TimeSpan.FromSeconds(0)) throw new ArgumentOutOfRangeException("duration", "duration must be greater than 0");

            iterations = 0;

            return null;
        }

        private static IBuffer Scrypt(string password, IBuffer salt) {
            return salt;
        }

        public static IBuffer Xor(IBuffer left, IBuffer right) {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Length != right.Length) throw new ArgumentException("buffers must have the same length");

            var leftArray = left.ToArray();
            var rightArray = right.ToArray();
            var result = new byte[left.Length];
            for (int i = 0; i < left.Length; ++i) {
                result[i] = (byte)(leftArray[i] ^ rightArray[i]);
            }
            return result.AsBuffer();
        }
    }
}
