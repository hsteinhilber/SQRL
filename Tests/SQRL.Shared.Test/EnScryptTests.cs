using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using WssStream = Windows.Storage.Streams.Buffer;

namespace SQRL.Test
{
    [TestClass]
    public class EnScryptTests {
        [TestMethod]
        public void It_should_throw_if_string_is_null() {
            var salt = new byte[] {};
            int iterations;
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.EnScrypt(null, salt, 1), 
                "EnScrypt with iteration specified did not throw an exception.");
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.EnScrypt(null, salt, TimeSpan.FromSeconds(5), out iterations),
                "EnScrypt that takes a duration did not throw an exception.");
        }

        [TestMethod]
        public void It_should_not_throw_if_string_is_empty() {
            var salt = new byte[] {};
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

        [DataTestMethod]
        [DataRow("", "", 1, "a8ea62a6e1bfd20e4275011595307aa302645c1801600ef5cd79bf9d884d911c")]
        [DataRow("", "", 100, "45a42a01709a0012a37b7b6874cf16623543409d19e7740ed96741d2e99aab67")]
        [DataRow("", "", 1000, "3f671adf47d2b1744b1bf9b50248cc71f2a58e8d2b43c76edb1d2a2c200907f5")]
        [DataRow("password", "", 123, "129d96d1e735618517259416a605be7094c2856a53c14ef7d4e4ba8e4ea36aeb")]
        [DataRow("password", "0000000000000000000000000000000000000000000000000000000000000000", 123, "2f30b9d4e5c48056177ff90a6cc9da04b648a7e8451dfa60da56c148187f6a7d")]
        public void It_should_compute_the_correct_hash(string password, string salt, int iterations, string expected) {
            var saltBuffer = CryptographicBuffer.DecodeFromHexString(salt).ToArray();

            var key = SecurityExtensions.EnScrypt(password, saltBuffer, iterations);
            var keyHex = CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(key));
            Assert.AreEqual(expected, keyHex, ignoreCase: true);
        }
    }
}
