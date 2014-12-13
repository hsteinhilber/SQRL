using System.Collections.Generic;
using Windows.Foundation.Metadata;

namespace Windows.ApplicationModel.Email {
    [MarshalingBehavior(MarshalingType.Agile)]
    [Threading(ThreadingModel.Both)]
    sealed class EmailMessage {
        public EmailMessage() {
            To = new List<EmailRecipient>();
            Cc = new List<EmailRecipient>();
            Bcc = new List<EmailRecipient>();
            Attachments = new List<EmailAttachment>();
            Subject = string.Empty;
            Body = string.Empty;
        }

        public IList<EmailRecipient> To { get; private set; }
        public IList<EmailRecipient> Cc { get; private set; }
        public IList<EmailRecipient> Bcc { get; private set; }
        public IList<EmailAttachment> Attachments { get; private set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
