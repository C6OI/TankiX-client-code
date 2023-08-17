using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupComponentCodec : NotOptionalCodec {
        Codec longCodec;

        Protocol protocol;

        [Inject] public static GroupRegistry GroupRegistry { get; set; }

        public override void Init(Protocol protocol) {
            longCodec = protocol.GetCodec(typeof(long));
            this.protocol = protocol;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            GroupComponent groupComponent = (GroupComponent)data;
            long uidByType = protocol.GetUidByType(groupComponent.GetType());
            longCodec.Encode(protocolBuffer, uidByType);
            longCodec.Encode(protocolBuffer, groupComponent.Key);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long uid = (long)longCodec.Decode(protocolBuffer);
            Type typeByUid = protocol.GetTypeByUid(uid);
            long key = (long)longCodec.Decode(protocolBuffer);
            return GroupRegistry.FindOrCreateGroup(typeByUid, key);
        }
    }
}