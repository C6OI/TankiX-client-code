using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentChangeCommandCodec : Codec {
        Codec entityCodec;

        Codec longCodec;
        Protocol protocol;

        public void Init(Protocol protocol) {
            entityCodec = protocol.GetCodec(typeof(EntityInternal));
            longCodec = protocol.GetCodec(typeof(long));
            this.protocol = protocol;
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            ComponentChangeCommand componentChangeCommand = (ComponentChangeCommand)data;
            entityCodec.Encode(protocolBuffer, componentChangeCommand.Entity);
            EncodeVaried(protocolBuffer, componentChangeCommand.Component);
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            ComponentChangeCommand componentChangeCommand = Activator.CreateInstance<ComponentChangeCommand>();
            DecodeToInstance(protocolBuffer, componentChangeCommand);
            return componentChangeCommand;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            ComponentChangeCommand componentChangeCommand = (ComponentChangeCommand)instance;
            EntityInternal entityInternal = (EntityInternal)entityCodec.Decode(protocolBuffer);
            long uid = (long)longCodec.Decode(protocolBuffer);
            Type typeByUid = protocol.GetTypeByUid(uid);
            Component component = null;
            component = !entityInternal.HasComponent(typeByUid) ? (Component)Activator.CreateInstance(typeByUid) : entityInternal.GetComponent(typeByUid);
            protocol.GetCodec(typeByUid).DecodeToInstance(protocolBuffer, component);
            componentChangeCommand.Entity = entityInternal;
            componentChangeCommand.Component = component;
        }

        void EncodeVaried(ProtocolBuffer protocolBuffer, object data) {
            Type type = data.GetType();
            long uidByType = protocol.GetUidByType(type);
            protocolBuffer.Writer.Write(uidByType);
            ProtocolBuffer protocolBuffer2 = protocol.NewProtocolBuffer();
            Codec codec = protocol.GetCodec(type);
            codec.Encode(protocolBuffer2, data);
            protocol.WrapPacket(protocolBuffer2, protocolBuffer.Data);
            protocol.FreeProtocolBuffer(protocolBuffer2);
        }
    }
}