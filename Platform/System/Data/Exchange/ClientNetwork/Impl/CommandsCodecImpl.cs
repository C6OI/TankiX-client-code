using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandsCodecImpl : Codec, CommandsCodec {
        readonly Dictionary<Type, CommandCode> codeByType = new();

        readonly TemplateRegistry templateRegistry;

        readonly Dictionary<CommandCode, Type> typeByCode = new();

        Codec commandCodeCodec;
        Protocol protocol;

        public CommandsCodecImpl(TemplateRegistry templateRegistry) {
            this.templateRegistry = templateRegistry;
            RegisterCommand<EntityShareCommand>(CommandCode.EntityShare);
            RegisterCommand<EntityUnshareCommand>(CommandCode.EntityUnshare);
            RegisterCommand<ComponentAddCommand>(CommandCode.ComponentAdd);
            RegisterCommand<ComponentRemoveCommand>(CommandCode.ComponentRemove);
            RegisterCommand<ComponentChangeCommand>(CommandCode.ComponentChange);
            RegisterCommand<SendEventCommand>(CommandCode.SendEvent);
            RegisterCommand<FlowBoundCommand>(CommandCode.FlowBound);
            RegisterCommand<InitTimeCommand>(CommandCode.InitTime);
            RegisterCommand<CloseCommand>(CommandCode.Close);
        }

        public void Init(Protocol protocol) {
            this.protocol = protocol;
            commandCodeCodec = protocol.GetCodec(typeof(CommandCode));
            protocol.RegisterCodecForType<TemplateAccessor>(new TemplateAccessorCodec(templateRegistry));
            protocol.RegisterCodecForType<Entity>(new EntityCodec());
            protocol.RegisterCodecForType<EntityInternal>(new EntityCodec());
            protocol.RegisterCodecForType<FlowBoundCommand>(new FlowBoundCommandCodec());
            protocol.RegisterInheritanceCodecForType<GroupComponent>(new GroupComponentCodec());
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
            CommandCode commandCode = (CommandCode)(byte)commandCodeCodec.Decode(protocolBuffer);
            Type value;

            if (!typeByCode.TryGetValue(commandCode, out value)) {
                throw new UnknownCommandException(commandCode);
            }

            Codec codec = protocol.GetCodec(value);
            return codec.Decode(protocolBuffer);
        }

        public void RegisterCommand<T>(CommandCode code) where T : Command {
            typeByCode.Add(code, typeof(T));
            codeByType.Add(typeof(T), code);
        }
    }
}