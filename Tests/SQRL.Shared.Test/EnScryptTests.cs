using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace SQRL.Test
{
    public static class SecurityExtensions {
        public static IBuffer EnScrypt(this string password, IBuffer salt, int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (iterations <= 0) throw new ArgumentOutOfRangeException("iterations", "iterations must be greater than 0");

            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesGcm);
            var pwdBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8) ?? 
                            CryptographicBuffer.CreateFromByteArray(new byte[] {});
            salt = salt ?? CryptographicBuffer.DecodeFromHexString("c0d2005faba6fc1eabd97761ee95e2902a222e49871be81ac92fd7c79fea2a1f"); // THIS IS WRONG!!!!
            return salt; // THIS IS WRONG TOO
        }

        public static IBuffer EnScrypt(this string password, IBuffer salt, TimeSpan duration, out int iterations) {
            if (password == null) throw new ArgumentNullException("password");
            if (duration <= TimeSpan.FromSeconds(0)) throw new ArgumentOutOfRangeException("duration", "duration must be greater than 0");

            iterations = 0;
            return null;
        }
    }

    [TestClass]
    public class EnScryptTests
    {
        [TestMethod]
        public void It_should_throw_if_string_is_null() {
            var salt = CryptographicBuffer.CreateFromByteArray(new byte[] {});
            int iterations;
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.EnScrypt(null, salt, 1), 
                "EnScrypt with iteration specified did not throw an exception.");
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.EnScrypt(null, salt, TimeSpan.FromSeconds(5), out iterations),
                "EnScrypt that takes a duration did not throw an exception.");
        }

        [TestMethod]
        public void It_should_not_throw_if_string_is_empty() {
            var salt = CryptographicBuffer.CreateFromByteArray(new byte[] {});
            int iterations;
            SecurityExtensions.EnScrypt("", salt, 1);
            SecurityExtensions.EnScrypt("", salt, TimeSpan.FromSeconds(5), out iterations);
        }

        [TestMethod]
        public void It_should_not_throw_if_salt_is_null() {
            int iterations;
            SecurityExtensions.EnScrypt("", null, 1);
            SecurityExtensions.EnScrypt("", null, TimeSpan.FromSeconds(5), out iterations);
        }

        [TestMethod]
        public void It_should_throw_if_iterations_is_negative_or_zero() {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecurityExtensions.EnScrypt("", null, -1),
                "did not throw on negative value for iteration");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecurityExtensions.EnScrypt("", null, 0),
                "did not throw on zero value for iteration");
        }

        [TestMethod]
        public void It_should_throw_if_duration_is_negative() {
            int iterations;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecurityExtensions.EnScrypt("", null, TimeSpan.FromSeconds(-1), out iterations),
                "did not throw on negative timespan for duration");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => SecurityExtensions.EnScrypt("", null, TimeSpan.FromSeconds(0), out iterations),
                "did not throw on zero timespan for duratrion");
        }

        [TestMethod]
        public void It_should_compute_the_correct_hash_given_no_password_and_no_salt() {
            var password = "";
            var salt = CryptographicBuffer.CreateFromByteArray(new byte[] { });
            var iterations = 57;
            var expected = "c0d2005faba6fc1eabd97761ee95e2902a222e49871be81ac92fd7c79fea2a1f";

            var key = SecurityExtensions.EnScrypt(password, salt, iterations);
            Assert.AreEqual(expected, CryptographicBuffer.EncodeToHexString(key), ignoreCase: true);
        }
    }
}
