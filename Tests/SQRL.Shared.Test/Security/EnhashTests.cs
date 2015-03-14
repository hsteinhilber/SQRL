using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQRL.Security.Test
{
    [TestClass]
    public class EnhashTests
    {
        [TestMethod]
        public void It_should_throw_if_source_array_is_null() {
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.Enhash(null));
        }

        [TestMethod]
        public void It_should_throw_if_source_array_is_not_256_bit() {
            Assert.ThrowsException<ArgumentException>(() => SecurityExtensions.Enhash(new byte[31]));
        }

        [TestMethod]
        public void It_should_return_a_256_bit_array() {
            var input = new byte[32];
            var result = SecurityExtensions.Enhash(input);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 32, "returned value is {0} bits", result.Length * 8);
        }

        [TestMethod]
        public void It_should_return_a_different_array_than_input() {
            var input = new byte[32];
            var result = SecurityExtensions.Enhash(input);
            Assert.AreNotSame(input, result);
            CollectionAssert.AreNotEqual(input, result);
        }
    }
}