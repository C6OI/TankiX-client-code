using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class DateTimeCodec : NotOptionalCodec {
        const int TICKS_IN_MILLISECOND = 10000;

        static readonly DateTime UNIX_EPOCH = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            DateTime dateTime = (DateTime)data;
            long value = (new DateTimeOffset(dateTime).UtcTicks - UNIX_EPOCH.Ticks) / 10000;
            protocolBuffer.Writer.Write(value);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long num = protocolBuffer.Reader.ReadInt64();
            DateTime dateTime = new(UNIX_EPOCH.Ticks + num * 10000, DateTimeKind.Utc);
            return dateTime;
        }
    }
}