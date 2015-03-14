using System;
using System.Text;
using Windows.Security.Cryptography.Core;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SQRL.Security {
    public static class SecurityExtensions {
        public static byte[] Enhash(this byte[] source) {
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != 32) throw new ArgumentException("source", "source must be a 256 bit array");

            var hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            byte[] input;
            // TODO: This is generating too many arrays for garbage collection.
            //       In addition, none of these allocated buffers are being cleared properly.
            var result = input = hasher.HashData(source.AsBuffer()).ToArray();
            for (int i = 1; i < 16; ++i) {
                input = hasher.HashData(input.AsBuffer()).ToArray();
                result.Xor(input);
            }

            return result;
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
