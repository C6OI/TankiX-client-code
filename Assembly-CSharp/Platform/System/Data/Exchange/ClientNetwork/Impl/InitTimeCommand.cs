using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class InitTimeCommand : Command {
        [ProtocolTransient] [Inject] public static ServerTimeServiceInternal ServerTimeService { get; set; }

        public long ServerTime { get; set; }

        public void Execute(Engine engine) {
            ServerTimeService.InitialServerTime = ServerTime;
        }
    }
}