using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQRL.Security.Test {
    [TestClass]
    public class SecureEqualTests {
        [DataTestMethod]
        [DataRow(new byte[] { }, new byte[] { })]
        [DataRow(new byte[] { 1, 2, 3, 4, 5 }, new byte[] { 1, 2, 3, 4, 5 })]
        [DataRow(new byte[] { 7, 9, 4, 3, 11, 12, 99 }, new byte[] { 7, 9, 4, 3, 11, 12, 99 })]
        public void It_should_return_equal_if_arrays_are_identical(byte[] left, byte[] right) {
            Assert.IsTrue(SecurityExtensions.SecureEquals(left, right), "arrays did not compare as equal");
        }

        [DataTestMethod]
        [DataRow(new byte[] { }, new byte[] { 1 })]
        [DataRow(new byte[] { 1, 2, 3, 4, 5 }, new byte[] { 6, 7, 8, 9, 10 })]
        [DataRow(new byte[] { 7, 9, 4, 3, 11, 12, 99 }, new byte[] { 7, 9, 4, 3, 11, 12 })]
        [DataRow(new byte[] { 1, 2, 3, 4, 5 }, new byte[] { 5, 4, 3, 2, 1 })]
        public void It_should_return_inequal_if_arrays_are_different(byte[] left, byte[] right) {
            Assert.IsFalse(SecurityExtensions.SecureEquals(left, right), "arrays compared as equal");
        }

        [TestMethod]
        public void It_should_return_equal_to_compare_an_array_against_itself() {
            var array = new byte[10];
            Assert.IsTrue(SecurityExtensions.SecureEquals(array, array));
        }

        [TestMethod]
        public void It_should_take_as_long_to_verify_inequal_as_it_takes_to_verify_equal() {
            SecurityExtensions.SecureEquals(new byte[20], new byte[20]); // Initial call to make sure the method is JIT'd for a fair comparison

            var first = new byte[0x800000];
            var second = new byte[0x1000];
            var inequalDuration = TimeMethodCall(() => SecurityExtensions.SecureEquals(first, second));

            second = new byte[0x800000];
            var equalDuration = TimeMethodCall(() => SecurityExtensions.SecureEquals(first, second));

            Assert.IsTrue(equalDuration - inequalDuration < TimeSpan.FromMilliseconds(10), "equal test took {0} while inequal test took {1}", equalDuration, inequalDuration);
        }

        private TimeSpan TimeMethodCall(Action method) {
            var start = DateTime.Now;
            method();
            return DateTime.Now - start;
        }
    }
}
