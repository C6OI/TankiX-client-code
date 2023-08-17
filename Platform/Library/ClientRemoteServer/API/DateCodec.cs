using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Library.ClientRemoteServer.API {
    public class DateCodec : NotOptionalCodec {
        [Inject] public static TimeService TimeService { get; set; }

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            long value = ((Date)data).ToServerTime(TimeService.DiffToServer);
            protocolBuffer.Writer.Write(value);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long num = protocolBuffer.Reader.ReadInt64();
            long diffToServer = TimeService.DiffToServer;
            float num2 = Date.FromServerTime(diffToServer, num);

            LoggerProvider.GetLogger(this).InfoFormat("Decode: serverTime={0} diffToServer={1} unityTime={2}",
                num,
                diffToServer,
                num2);

            return new Date(num2);
        }
    }
}