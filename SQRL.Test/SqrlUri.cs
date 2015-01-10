using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQRL.Test {
    public class SqrlUri : Uri {
        public SqrlUri(string uri) : base(uri.Replace("|","/")) {
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
    }
}
