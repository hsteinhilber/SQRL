using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using WssBuffer = Windows.Storage.Streams.Buffer;

namespace SQRL.Test
{
    [TestClass]
    public class XorTests {
        [TestMethod]
        public void It_should_throw_if_values_are_null() {
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.Xor(null, new byte[1]));
            Assert.ThrowsException<ArgumentNullException>(() => SecurityExtensions.Xor(new byte[1], null));
        }

        [TestMethod]
        public void It_should_throw_if_buffers_differ_in_length() {
            var left = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var right = new byte[] { 0x01, 0x02, 0x03 };
            Assert.ThrowsException<ArgumentException>(() => SecurityExtensions.Xor(left, right));
        }

        [DataTestMethod]
        [DataRow("0,0,0,0", "0,0,0,0", "0,0,0,0")]
        [DataRow("255,255,255,255", "255,255,255,255", "0,0,0,0")]
        [DataRow("85,170,85,170", "170,85,170,85", "255,255,255,255")]
        [DataRow("85,85,85,85", "255,255,255,255", "170,170,170,170")]
        public void It_should_xor_each_byte_of_the_arrays_to_produce_a_new_byte(string left, string right, string result) {
            var leftBuffer = StringToByteArray(left);
            var rightBuffer = StringToByteArray(right);
            var expected = StringToByteArray(result);

            var actual = SecurityExtensions.Xor(leftBuffer, rightBuffer);
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void It_should_overwrite_the_contents_of_the_first_array() {
            var left = new byte[] { 0x00, 0x01, 0x02, 0x03 };
            var right = new byte[] { 0x1, 0x02, 0x03, 0x04 };

            var result = SecurityExtensions.Xor(left, right);

            Assert.AreSame(left, result);
        }

        private byte[] StringToByteArray(string text) {
            var values = from value in text.Split(',')
                         select byte.Parse(value);
            return values.ToArray();
        }
    }
}
