using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientTimeComponent : Component {
        public long PingServerTime { get; set; }

        public float PingClientRealTime { get; set; }
    }
}