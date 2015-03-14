using System;
using System.Text;
using Windows.Security.Cryptography.Core;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SQRL.Security {
    public static class SecurityExtensions {
        public static byte[] Enhash(this byte[] source) {
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != 32) throw new ArgumentException("source", "source must be a 256 bit array");

            // TODO: This is generating too many arrays for garbage collection.
            //       In addition, none of these allocated buffers are being cleared properly.
            var hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            return source.Pbkdf2((k, s) => hasher.HashData(s.AsBuffer()).ToArray(), source, 16);
        }

        public static byte[] Pbkdf2(this byte[] password, Func<byte[], byte[], byte[]> hash, byte[] salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations < 1) throw new ArgumentOutOfRangeException("iterations", iterations, "iterations must be a positive integer");

            // TODO: key and cloned salt need to be wiped before going out of scope
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
    }
}
