using System;
using System.Collections.Generic;
using System.Linq;

namespace SQRL {
    public class SqrlUri : Uri {
        readonly Dictionary<string, int> SUPPORTED_SCHEMES = new Dictionary<string, int>() { { "sqrl", 443 }, { "qrl", 80 } };

        public SqrlUri(string uri) : base(uri.Replace("|","/")) {
            if (!SUPPORTED_SCHEMES.Keys.Contains(base.Scheme))
                throw new NotSupportedException(String.Format("Scheme '{0}' is not supported", base.Scheme));
          
            if (uri.Contains("|")) {
                int hostIndex = uri.IndexOf(base.Host);
                int startIndex = uri.IndexOf("/", hostIndex);
                int length = uri.IndexOf("|") - startIndex;
                _siteKeyString = base.Host + uri.Substring(startIndex, length);
            }
        }

        private string _siteKeyString;
        public string SiteKeyString {
            get {
                return _siteKeyString ?? base.Host;
            }
        }

        public new int Port {
            get {
                return base.Port > 0 ? base.Port : SUPPORTED_SCHEMES[base.Scheme];
            }
        }
    }
}
