using System;

namespace SQRL.Security {
    public static class SecurityExtensions {
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
