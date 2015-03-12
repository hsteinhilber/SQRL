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
        [DataRow("", "", 10, "007247087801b8d77c2e47e6da6759ea63e38c81b64742f569ead005e75fe060")]
        [DataRow("password", "", 10, "08cde5572414a6f6d83a881e52f368ea448930207b309319379304316b44e057")]
        [DataRow("password", "0000000000000000000000000000000000000000000000000000000000000000", 10, "ba6006e4c23262a8cc7f242e110cc644e7ad8237e2b0a2265fbe6d51148c27b3")]
        public void Enscrypt_should_compute_the_correct_hash(string password, string saltText, int iterations, string expected) {
            var hasher = new PasswordHasher(password);
            var salt = saltText == "" ? new byte[0] : CryptographicBuffer.DecodeFromHexString(saltText).ToArray();

            var key = hasher.EnScrypt(salt, iterations);
            var keyHex = CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(key));
            Assert.AreEqual(expected, keyHex, ignoreCase: true);
        }
    }
}
