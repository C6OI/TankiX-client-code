using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandsCodecImpl : CommandsCodec, Codec {
        readonly Dictionary<Type, CommandCode> codeByType = new();

        Codec commandCodeCodec;
        Protocol protocol;

        readonly TemplateRegistry templateRegistry;

        readonly Dictionary<CommandCode, Type> typeByCode = new();

        public CommandsCodecImpl(TemplateRegistry templateRegistry) {
            this.templateRegistry = templateRegistry;
            RegisterCommand<EntityShareCommand>(CommandCode.EntityShare);
            RegisterCommand<EntityUnshareCommand>(CommandCode.EntityUnshare);
            RegisterCommand<ComponentAddCommand>(CommandCode.ComponentAdd);
            RegisterCommand<ComponentRemoveCommand>(CommandCode.ComponentRemove);
            RegisterCommand<ComponentChangeCommand>(CommandCode.ComponentChange);
            RegisterCommand<SendEventCommand>(CommandCode.SendEvent);
            RegisterCommand<InitTimeCommand>(CommandCode.InitTime);
            RegisterCommand<CloseCommand>(CommandCode.Close);
        }

        [Inject] public static ProtocolFlowInstancesCache ProtocolFlowInstances { get; set; }

        public void Init(Protocol protocol) {
            this.protocol = protocol;
            commandCodeCodec = protocol.GetCodec(typeof(CommandCode));
            protocol.RegisterCodecForType<TemplateAccessor>(new TemplateAccessorCodec(templateRegistry));
            protocol.RegisterCodecForType<Entity>(new EntityCodec());
            protocol.RegisterCodecForType<EntityInternal>(new EntityCodec());
            protocol.RegisterInheritanceCodecForType<GroupComponent>(new GroupComponentCodec());
            protocol.RegisterCodecForType<ComponentChangeCommand>(new ComponentChangeCommandCodec());
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            Command command = (Command)data;
            Type type = command.GetType();
            CommandCode commandCode = codeByType[type];
            Codec codec = protocol.GetCodec(type);
            commandCodeCodec.Encode(protocolBuffer, commandCode);
            codec.Encode(protocolBuffer, command);
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            CommandCode commandCode = (CommandCode)commandCodeCodec.Decode(protocolBuffer);
            Type value;

            if (!typeByCode.TryGetValue(commandCode, out value)) {
                throw new UnknownCommandException(commandCode);
            }

            Codec codec = protocol.GetCodec(value);
            object instance = ProtocolFlowInstances.GetInstance(value);
            codec.DecodeToInstance(protocolBuffer, instance);
            return instance;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            throw new NotImplementedException();
        }

        public void RegisterCommand<T>(CommandCode code) where T : Command {
            typeByCode.Add(code, typeof(T));
            codeByType.Add(typeof(T), code);
        }
    }
}