using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace SQRL.Security.Test
{
    [TestClass]
    public class ClearTests
    {
        [TestMethod]
        public void It_should_not_throw_if_source_is_null() {
            SecurityExtensions.Clear((byte[])null);
        }

        [TestMethod]
        public void It_should_not_throw_if_source_is_empty() {
            SecurityExtensions.Clear(new byte[0]);
        }

        [TestMethod]
        public void It_should_set_all_elements_in_array_to_zero() {
            var source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            SecurityExtensions.Clear(source);
            CollectionAssert.AreEqual(new byte[10], source);
        }

        [TestMethod]
        public void It_should_not_throw_if_source_buffer_is_null() {
            SecurityExtensions.Clear((IBuffer)null);
        }

        [TestMethod]
        public void It_should_not_throw_if_source_buffer_is_empty() {
            SecurityExtensions.Clear(new Windows.Storage.Streams.Buffer(0));
        }

        [TestMethod]
        public void It_should_set_all_elements_in_buffer_to_zero() {
            var source = CryptographicBuffer.CreateFromByteArray(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            SecurityExtensions.Clear(source);
            var expected = CryptographicBuffer.CreateFromByteArray(new byte[10]);
            Assert.IsTrue(CryptographicBuffer.Compare(expected, source), "source buffer does not compare equal to a buffer containing only 0's");
        }


    }
}
