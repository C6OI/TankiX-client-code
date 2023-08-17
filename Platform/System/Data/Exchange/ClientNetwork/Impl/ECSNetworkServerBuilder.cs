using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientProtocol.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ECSNetworkServerBuilder {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public static NetworkServiceImpl Build(EngineServiceInternal engineServiceInternal, Protocol protocol,
            Socket socket) {
            ServiceRegistry.Current.RegisterService((ClientProtocolInstancesCache)new ClientProtocolInstancesCacheImpl());
            ServiceRegistry.Current.RegisterService((ClientNetworkInstancesCache)new ClientNetworkInstancesCacheImpl());
            ComponentAndEventRegistrator componentAndEventRegistrator = new(engineServiceInternal, protocol);
            SharedEntityRegistry sharedEntityRegistry = new SharedEntityRegistryImpl(engineServiceInternal);
            ServiceRegistry.Current.RegisterService(sharedEntityRegistry);
            CommandsCodecImpl commandsCodecImpl = new(TemplateRegistry);
            commandsCodecImpl.Init(protocol);
            ServiceRegistry.Current.RegisterService((CommandsCodec)commandsCodecImpl);

            return Build(engineServiceInternal,
                protocol,
                socket,
                componentAndEventRegistrator,
                sharedEntityRegistry,
                commandsCodecImpl);
        }

        public static NetworkServiceImpl Build(EngineServiceInternal engineServiceInternal, Protocol protocol, Socket socket,
            ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry,
            CommandsCodec commandsCodec) {
            NetworkServiceImpl networkServiceImpl = new(socket, new ProtocolAdapterImpl(protocol, commandsCodec));
            new CommandsSender(engineServiceInternal, networkServiceImpl, componentAndEventRegistrator, entityRegistry);
            new InCommandProcessor(engineServiceInternal, networkServiceImpl);
            return networkServiceImpl;
        }
    }
}