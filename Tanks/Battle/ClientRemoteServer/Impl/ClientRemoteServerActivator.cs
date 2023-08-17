using System;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientRemoteServer.API;
using Platform.Library.ClientResources.Impl;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.Library.ClientUnityIntegration.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Platform.System.Data.Exchange.ClientNetwork.Impl;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class ClientRemoteServerActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        NetworkServiceImpl networkService;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [Inject] public static Protocol Protocol { get; set; }

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [Inject] public static ServerTimeService ServerTimeService { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new TimeSyncSystem());
            EngineService.RegisterSystem(new TankMovementReceiverSystem());
            EngineService.RegisterSystem(new TankMovementSenderSystem());
            EngineService.RegisterSystem(new WallhackSystem());
        }

        protected override void Activate() {
            TimeServiceImpl timeServiceImpl = new();
            ServiceRegistry.Current.RegisterService((TimeService)timeServiceImpl);
            ServerTimeService.OnInitServerTime += timeServiceImpl.InitServerTime;
            Protocol.RegisterCodecForType<Movement>(new MovementCodec());
            Protocol.RegisterCodecForType<MoveCommand>(new MoveCommandCodec());
            Protocol.RegisterCodecForType<Date>(new DateCodec());
            string host = InitConfiguration.Config.Host;
            string[] source = new string[1] { InitConfiguration.Config.AcceptorPort };
            PrefetchSocketPolicy(host);
            ServerConnectionBehaviour serverConnectionBehaviour = gameObject.AddComponent<ServerConnectionBehaviour>();
            serverConnectionBehaviour.OpenConnection(host, source.Select(x => Convert.ToInt32(x)).ToArray());
        }

        void PrefetchSocketPolicy(string hostName) { }
    }
}