using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class UnknownCommandException : Exception {
        public UnknownCommandException(CommandCode commandCode)
            : base(string.Format("Unknown command code {0}.", commandCode)) { }
    }
}