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

            return source.Pbkdf2((r, k, s) => Sha256Hash(r, s), source, 16, 32);
        }

        public static byte[] Pbkdf2(this byte[] password, Action<byte[], byte[], byte[]> hash, byte[] salt, int iterations, int dkLen) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations < 1) throw new ArgumentOutOfRangeException("iterations", iterations, "iterations must be a positive integer");

            var result = new byte[dkLen];
            var next = new byte[dkLen];
            var key = (byte[])password.Clone();
            salt = salt == null ? new byte[0] : (byte[])salt.Clone();

            hash(next, key, salt);
            next.CopyTo(result, 0);
            for (int i = 1; i < iterations; ++i) {
                hash(next, key, next);
                result.Xor(next);
            }

            key.Clear(); salt.Clear(); next.Clear();

            return result;
        }

        public static byte[] Pbkdf2(this string password, Action<byte[], byte[], byte[]> hash, byte[] salt, int iterations, int dkLen) {
            if (password == null) throw new ArgumentNullException("password");
            var pwdArray = Encoding.UTF8.GetBytes(password);
            // TODO: pwdArray needs to be wiped before going out of scope
            return pwdArray.Pbkdf2(hash, salt, iterations, dkLen);
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

        public static bool SecureEquals(this byte[] left, byte[] right) {
            var result = left.Length ^ right.Length;
            for (int i = 0; i < left.Length; ++i) {
                result |= left[i] ^ right[i % right.Length];
            }
            return result == 0;
        }

        private static HashAlgorithmProvider sha256hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);

        private static void Sha256Hash(byte[] result, byte[] salt) {
            var buffer = salt.AsBuffer();
            var hash = sha256hasher.HashData(buffer);
            hash.CopyTo(result);
            hash.Clear();
        }
    }
}
