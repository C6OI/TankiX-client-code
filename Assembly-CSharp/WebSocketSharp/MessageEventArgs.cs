using System;

namespace WebSocketSharp {
    public class MessageEventArgs : EventArgs {
        string _data;

        bool _dataSet;

        internal MessageEventArgs(WebSocketFrame frame) {
            Type = frame.Opcode;
            RawData = frame.PayloadData.ApplicationData;
        }

        internal MessageEventArgs(Opcode opcode, byte[] rawData) {
            if ((ulong)rawData.LongLength > PayloadData.MaxLength) {
                throw new WebSocketException(CloseStatusCode.TooBig);
            }

            Type = opcode;
            RawData = rawData;
        }

        public string Data {
            get {
                if (!_dataSet) {
                    _data = Type == Opcode.Binary ? BitConverter.ToString(RawData) : RawData.UTF8Decode();
                    _dataSet = true;
                }

                return _data;
            }
        }

        public bool IsBinary => Type == Opcode.Binary;

        public bool IsPing => Type == Opcode.Ping;

        public bool IsText => Type == Opcode.Text;

        public byte[] RawData { get; }

        [Obsolete("This property will be removed. Use any of the Is properties instead.")]
        public Opcode Type { get; }
    }
}