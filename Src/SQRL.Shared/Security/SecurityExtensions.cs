using System;
using System.Text;
using Windows.Security.Cryptography.Core;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using WssBuffer = Windows.Storage.Streams.Buffer;
using System.IO;

namespace SQRL.Security {
    public static class SecurityExtensions {
        public static byte[] Enhash(this byte[] source) {
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != 32) throw new ArgumentException("source", "source must be a 256 bit array");

            return source.Pbkdf2((k, s) => Sha256Hash(s), source, 16);
        }

        private static HashAlgorithmProvider sha256hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
        private static byte[] Sha256Hash(byte[] salt) {
            var buffer = salt.AsBuffer();
            var hash =  sha256hasher.HashData(buffer);
            var result = hash.ToArray();
            buffer.Clear();
            hash.Clear();
            return result;
        }

        public static byte[] Pbkdf2(this byte[] password, Func<byte[], byte[], byte[]> hash, byte[] salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations < 1) throw new ArgumentOutOfRangeException("iterations", iterations, "iterations must be a positive integer");

            // TODO: key and cloned salt need to be wiped before going out of scope
            // TODO: specify derived key length and pass buffer to hash method instead of taking return value
            var key = (byte[])password.Clone();
            salt = salt == null ? new byte[0] : (byte[])salt.Clone();
            byte[] result = salt = hash(key, salt);
            for (int i = 1; i < iterations; ++i) {
                salt = hash(key, salt);
                result.Xor(salt);
            }
            return result;
        }

        public static byte[] Pbkdf2(this string password, Func<byte[], byte[], byte[]> hash, byte[] salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            var pwdArray = Encoding.UTF8.GetBytes(password);
            // TODO: pwdArray needs to be wiped before going out of scope
            return pwdArray.Pbkdf2(hash, salt, iterations);
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

        public static void Clear(this byte[] source) {
            if (source == null) return;
            for (int i = 0; i < source.Length; ++i)
                source[i] = 0;
        }

        public static void Clear(this IBuffer source) {
            if (source == null) return;
            using (var stream = source.AsStream()) {
                stream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < source.Length; ++i)
                    stream.WriteByte(0);
            }
        }
    }
}
