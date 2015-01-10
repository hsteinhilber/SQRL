using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SQRL.Test {
    [TestFixture]
    public class SqrlUriTests {
        [TestCase("sqrl://amazon.com/sqrl/login.php?NONCE", "amazon.com")]
        [TestCase("sqrl://amazon.com:8080/sqrl/login.php?NONCE", "amazon.com")]
        [TestCase("sqrl://amazon.com/hsteinhilber|sqrl/login.php?NONCE", "amazon.com/hsteinhilber")]
        [TestCase("sqrl://amazon.com:8080/hsteinhilber|sqrl/login.php?NONCE", "amazon.com/hsteinhilber")]
        public void SiteKeyString_should_be_correct(string uriString, string siteKeyString) {
            SqrlUri uri = new SqrlUri(uriString);
            Assert.That(uri.SiteKeyString, Is.EqualTo(siteKeyString));
        }

        [Test]
        public void Value_should_not_include_pipe_character_when_used() {
            SqrlUri uri = new SqrlUri("sqrl://amazon.com/hsteinhilber|sqrl/login.php?NONCE");
            Assert.That(uri.ToString(), Is.EqualTo("sqrl://amazon.com/hsteinhilber/sqrl/login.php?NONCE"));
        }
    }
}
