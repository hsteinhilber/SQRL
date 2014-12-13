using System;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Windows.ApplicationModel.Email {
    [MarshalingBehavior(MarshalingType.Standard)]
    [Threading(ThreadingModel.Both)]
    static class EmailManager {
        public static IAsyncAction ShowComposeNewEmailAsync(EmailMessage msg) {
            throw new NotImplementedException("Sending email from Windows 8.1 client is not currently supported.");
            return null;
        } 
    }
}
