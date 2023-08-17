using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EntityCodec : NotOptionalCodec {
        Codec longCodec;

        [Inject] public static SharedEntityRegistry SharedEntityRegistry { get; set; }

        public override void Init(Protocol protocol) => longCodec = protocol.GetCodec(typeof(long));

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            longCodec.Encode(protocolBuffer, ((Entity)data).Id);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long entityId = (long)longCodec.Decode(protocolBuffer);
            EntityInternal entity;

            if (SharedEntityRegistry.TryGetEntity(entityId, out entity)) {
                return entity;
            }

            return SharedEntityRegistry.CreateEntity(entityId);
        }
    }
}