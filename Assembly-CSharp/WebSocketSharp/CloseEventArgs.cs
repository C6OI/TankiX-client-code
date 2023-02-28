using System;

namespace WebSocketSharp {
    public class CloseEventArgs : EventArgs {
        PayloadData _payloadData;

        readonly string _reason;

        internal CloseEventArgs() {
            Code = 1005;
            _payloadData = PayloadData.Empty;
        }

        internal CloseEventArgs(ushort code) => Code = code;

        internal CloseEventArgs(CloseStatusCode code)
            : this((ushort)code) { }

        internal CloseEventArgs(PayloadData payloadData) {
            _payloadData = payloadData;
            byte[] applicationData = payloadData.ApplicationData;
            int num = applicationData.Length;
            Code = (ushort)(num <= 1 ? 1005 : applicationData.SubArray(0, 2).ToUInt16(ByteOrder.Big));
            _reason = num <= 2 ? string.Empty : applicationData.SubArray(2, num - 2).UTF8Decode();
        }

        internal CloseEventArgs(ushort code, string reason) {
            Code = code;
            _reason = reason;
        }

        internal CloseEventArgs(CloseStatusCode code, string reason)
            : this((ushort)code, reason) { }

        internal PayloadData PayloadData => _payloadData ?? (_payloadData = new PayloadData(Code.Append(_reason)));

        public ushort Code { get; }

        public string Reason => _reason ?? string.Empty;

        public bool WasClean { get; internal set; }
    }
}