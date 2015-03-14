using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQRL.Security.Test
{
    [TestClass]
    public class Pbkdf2Tests
    {
        [TestMethod]
        public void It_should_throw_if_password_is_null() {
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.Pbkdf2((byte[])null, (k,s) => s, new byte[0], 1));
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.Pbkdf2((string)null, (k,s) => s, new byte[0], 1));
        }

        [TestMethod]
        public void It_should_return_the_Hmac_if_only_one_iteration_is_specified() {
            var salt = new byte[] { 0, 1, 2, 3, 4, 5 };
            var result = SecurityExtensions.Pbkdf2(new byte[0], (k,s) => s, salt, 1);
            CollectionAssert.AreEqual(salt, result);
        }

        [TestMethod]
        public void It_should_pass_key_and_salt_to_Hmac_first_iteration() {
            var key = new byte[] { 1, 2, 3, 4, 5 };
            var salt = new byte[] { 6, 7, 8, 9, 0 };
            bool hmacCalled = false;
            SecurityExtensions.Pbkdf2(key, (k, s) => {
                CollectionAssert.AreEqual(key, k);
                CollectionAssert.AreEqual(salt, s);
                hmacCalled = true;
                return s;
            }, salt, 1);
            Assert.IsTrue(hmacCalled, "HMAC function was not called to verify key/salt values");
        }

        [TestMethod]
        public void It_should_pass_the_result_of_Hmac_as_salt_to_next_iteration() {
            int hmacCalled = 0;
            var salt = new byte[] { 1, 2, 3, 4, 5 };
            var newSalt = new byte[] { 6, 7, 8, 9, 10 };
            SecurityExtensions.Pbkdf2(new byte[0], (k,s) => {
                if (hmacCalled == 0) {
                    CollectionAssert.AreEqual(salt, s);
                } else {
                    CollectionAssert.AreEqual(newSalt, s);
                }
                hmacCalled++;
                return newSalt;
            }, salt, 2);
            Assert.IsTrue(hmacCalled == 2, "HMAC function was not called the correct number of times");
        }

        [TestMethod]
        public void It_should_xor_the_results_of_the_hmac_to_produce_a_result() {
            var salts = new byte[][] { new byte[] { 0x01, 0x01, 0x01, 0x01 }, 
                                       new byte[] { 0x02, 0x01, 0x02, 0x02 }, 
                                       new byte[] { 0x04, 0x01, 0x03, 0x01 } };
            var expected = new byte[] { 0x07, 0x01, 0x00, 0x02 };
            int iteration = 0;
            var result = SecurityExtensions.Pbkdf2(new byte[0], (k,s) => {
                return salts[iteration++];
            }, new byte[0], 3);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void It_should_not_assume_non_modification_of_key_and_salt_by_Hmac() {
            var key = new byte[] { 1, 2, 3, 4 };
            var salt = new byte[] { 5, 6, 7, 8 };
            SecurityExtensions.Pbkdf2(key, (k, s) => {
                CollectionAssert.AreEqual(key, k, "key does not match passed parameter");
                Assert.AreNotSame(key, k, "key references the same array as passed parameter");
                CollectionAssert.AreEqual(salt, s, "salt does not match passed parameter");
                Assert.AreNotSame(salt, s, "salt references the same array as passed parameter");
                return new byte[0];
            }, salt, 1);
        }
    }
}
