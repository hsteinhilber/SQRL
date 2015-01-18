using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SQRL.Test {
    [TestFixture]
    public class AesGcmTests {
        [Test]
        public void Gcm_block_mode_should_be_an_implementation_of_Aes() {
            var gcm = new AesGcm();
            Assert.That(gcm, Is.InstanceOf<Aes>());
        }
    }
}
