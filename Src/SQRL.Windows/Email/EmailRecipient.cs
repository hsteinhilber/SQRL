using Windows.Foundation.Metadata;

namespace Windows.ApplicationModel.Email {
    [MarshalingBehavior(MarshalingType.Agile)]
    [Threading(ThreadingModel.Both)]
    sealed class EmailRecipient {
        public EmailRecipient(string name, string address) {
            Address = address;
            Name = name;
        }
        public EmailRecipient(string address) : this(string.Empty, address) { }
        public EmailRecipient() : this(string.Empty, string.Empty) { }

        public string Address { get; set; }
        public string Name { get; set; }
    }
}
