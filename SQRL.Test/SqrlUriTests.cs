using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SQRL.Test {
    [TestFixture]
    public class SqrlUriTests {
        [TestCase("sqrl://example.com/sqrl/login.php?NONCE", "example.com")]
        [TestCase("sqrl://example.com:8080/sqrl/login.php?NONCE", "example.com")]
        [TestCase("sqrl://example.com/hsteinhilber|sqrl/login.php?NONCE", "example.com/hsteinhilber")]
        [TestCase("sqrl://example.com:8080/hsteinhilber|sqrl/login.php?NONCE", "example.com/hsteinhilber")]
        public void SiteKeyString_should_be_correct(string uriString, string siteKeyString) {
            var uri = new SqrlUri(uriString);
            Assert.That(uri.SiteKeyString, Is.EqualTo(siteKeyString));
        }

        [TestCase("sqrl://example.com", 443)]
        [TestCase("qrl://example.com", 80)]
        [TestCase("sqrl://example.com:8080", 8080)]
        [TestCase("qrl://example.com:1024", 1024)]
        public void Port_should_be_correct(string uriString, int expectedPort) {
            var uri = new SqrlUri(uriString);
            Assert.That(uri.Port, Is.EqualTo(expectedPort));
        }

        [TestCase("http://www.example.com")]
        [TestCase("https://www.example.com")]
        [TestCase("mailto:test@example.com")]
        [TestCase("ftp://www.example.com")]
        public void Unsupported_schemes_should_throw_an_exception(string uriString) {
            Assert.That(() => new SqrlUri(uriString), Throws.InstanceOf<NotSupportedException>());
        }

        [TestCase("sqrl://example.com/")]
        [TestCase("qrl://example.com/")]
        public void Supported_schemes_should_include_sqrl_and_qrl(string uriString) {
            Assert.That(() => new SqrlUri(uriString), Throws.Nothing);
        }

        [TestCase("SQRL://example.com/", "sqrl://example.com/")]
        [TestCase("sqrl://Example.Com/", "sqrl://example.com/")]
        public void Scheme_and_host_should_always_be_lowercase(string uriString, string expected) {
            var uri = new SqrlUri(uriString);
            Assert.That(uri.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void Path_should_retain_casing() {
            var expected = "sqrl://example.com/PathWithCasing?Id=SomeNonce";
            var uri = new SqrlUri(expected);
            Assert.That(uri.ToString(), Is.EqualTo(expected));
        }
    }
}
