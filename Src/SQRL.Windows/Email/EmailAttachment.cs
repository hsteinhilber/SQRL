using Windows.Foundation.Metadata;
using Windows.Storage.Streams;

namespace Windows.ApplicationModel.Email {
    [MarshalingBehavior(MarshalingType.Agile)]
    [Threading(ThreadingModel.Both)]
    sealed class EmailAttachment {
        public EmailAttachment(string fileName, IRandomAccessStreamReference data) {
            FileName = fileName;
            Data = data;
        }
        public EmailAttachment() : this(string.Empty, null) { }

        public IRandomAccessStreamReference Data { get; set; }
        public string FileName { get; set; }
    }
}
