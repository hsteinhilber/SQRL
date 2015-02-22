using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace SQRL.Test {
    [TestClass]
    public class SqrlUriTests {
        [DataTestMethod]
        [DataRow("sqrl://example.com/sqrl/login.php?NONCE", "example.com")]
        [DataRow("sqrl://example.com:8080/sqrl/login.php?NONCE", "example.com")]
        [DataRow("sqrl://example.com/hsteinhilber|sqrl/login.php?NONCE", "example.com/hsteinhilber")]
        [DataRow("sqrl://example.com:8080/hsteinhilber|sqrl/login.php?NONCE", "example.com/hsteinhilber")]
        public void SiteKeyString_should_be_correct(string uriString, string siteKeyString) {
            var uri = new SqrlUri(uriString);
            Assert.AreEqual(siteKeyString, uri.SiteKeyString, true);
        }

        [DataTestMethod]
        [DataRow("sqrl://example.com", 443)]
        [DataRow("qrl://example.com", 80)]
        [DataRow("sqrl://example.com:8080", 8080)]
        [DataRow("qrl://example.com:1024", 1024)]
        public void Port_should_be_correct(string uriString, int expectedPort) {
            var uri = new SqrlUri(uriString);
            Assert.AreEqual(expectedPort, uri.Port);
        }

        [DataTestMethod]
        [DataRow("http://www.example.com")]
        [DataRow("https://www.example.com")]
        [DataRow("mailto:test@example.com")]
        [DataRow("ftp://www.example.com")]
        public void Unsupported_schemes_should_throw_an_exception(string uriString) {
            Assert.ThrowsException<NotSupportedException>(() => new SqrlUri(uriString));
        }

        [DataTestMethod]
        [DataRow("sqrl://example.com/")]
        [DataRow("qrl://example.com/")]
        public void Supported_schemes_should_include_sqrl_and_qrl(string uriString) {
            var uri = new SqrlUri(uriString);
        }

        [DataTestMethod]
        [DataRow("SQRL://example.com/", "sqrl://example.com/")]
        [DataRow("sqrl://Example.Com/", "sqrl://example.com/")]
        public void Scheme_and_host_should_always_be_lowercase(string uriString, string expected) {
            var uri = new SqrlUri(uriString);
            Assert.AreEqual(expected, uri.ToString());
        }

        [TestMethod]
        public void Path_should_retain_casing() {
            var expected = "sqrl://example.com/PathWithCasing?Id=SomeNonce";
            var uri = new SqrlUri(expected);
            Assert.AreEqual(expected, uri.ToString());
        }
    }
}
