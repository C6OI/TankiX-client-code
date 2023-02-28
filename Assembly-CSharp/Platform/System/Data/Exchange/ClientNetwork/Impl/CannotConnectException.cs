using System;
using System.Linq;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CannotConnectException : Exception {
        public CannotConnectException(string host, params int[] ports)
            : base(string.Format("host={0}, ports={1}", host, string.Join(",", ports.Select(port => port.ToString()).ToArray()))) { }
    }
}