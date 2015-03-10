using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;

namespace SQRL.Security.Test {
    [TestClass]
    public class PasswordHasherTests {
        [TestMethod]
        public void Password_should_be_empty_string_if_created_with_null() {
            var hasher = new PasswordHasher(null);
            Assert.AreEqual("", hasher.Password);
        }

        [TestMethod]
        public void Enscrypt_should_not_throw_if_password_is_empty() {
            var salt = new byte[] { };
            var hasher = new PasswordHasher("");
            int iterations;

            hasher.EnScrypt(salt, 1);
            hasher.EnScrypt(salt, TimeSpan.FromSeconds(5), out iterations);
        }

        [TestMethod]
        public void Enscrypt_should_not_throw_if_salt_is_null() {
            var hasher = new PasswordHasher("password");
            int iterations;
            hasher.EnScrypt(null, 1);
            hasher.EnScrypt(null, TimeSpan.FromSeconds(5), out iterations);
        }

        [TestMethod]
        public void Enscrypt_should_throw_if_iterations_is_negative_or_zero() {
            var hasher = new PasswordHasher("password");
            var salt = new byte[] { };
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hasher.EnScrypt(salt, -1),
                "did not throw on negative value for iteration");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hasher.EnScrypt(salt, 0),
                "did not throw on zero value for iteration");
        }

        [TestMethod]
        public void Enscrypt_should_throw_if_duration_is_negative() {
            var hasher = new PasswordHasher("password");
            var salt = new byte[] { };
            int iterations;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hasher.EnScrypt(salt, TimeSpan.FromSeconds(-1), out iterations),
                "did not throw on negative timespan for duration");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hasher.EnScrypt(salt, TimeSpan.FromSeconds(0), out iterations),
                "did not throw on zero timespan for duratrion");
        }

        [DataTestMethod]
        [DataRow("", "", 1, "a8ea62a6e1bfd20e4275011595307aa302645c1801600ef5cd79bf9d884d911c")]
        [DataRow("", "", 100, "45a42a01709a0012a37b7b6874cf16623543409d19e7740ed96741d2e99aab67")]
        [DataRow("", "", 1000, "3f671adf47d2b1744b1bf9b50248cc71f2a58e8d2b43c76edb1d2a2c200907f5")]
        [DataRow("password", "", 123, "129d96d1e735618517259416a605be7094c2856a53c14ef7d4e4ba8e4ea36aeb")]
        [DataRow("password", "0000000000000000000000000000000000000000000000000000000000000000", 123, "2f30b9d4e5c48056177ff90a6cc9da04b648a7e8451dfa60da56c148187f6a7d")]
        public void Enscrypt_should_compute_the_correct_hash(string password, string saltText, int iterations, string expected) {
            var hasher = new PasswordHasher(password);
            var salt = saltText == "" ? new byte[0] : CryptographicBuffer.DecodeFromHexString(saltText).ToArray();

            var key = hasher.EnScrypt(salt, iterations);
            var keyHex = CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(key));
            Assert.AreEqual(expected, keyHex, ignoreCase: true);
        }
    }
}
