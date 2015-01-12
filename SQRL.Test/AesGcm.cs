using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SQRL.Test {
    public class AesGcm : Aes {
        public AesGcm() {
            
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV) {
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV) {
            throw new NotImplementedException();
        }

        public override void GenerateIV() {
            throw new NotImplementedException();
        }

        public override void GenerateKey() {
            throw new NotImplementedException();
        }
    }
}
