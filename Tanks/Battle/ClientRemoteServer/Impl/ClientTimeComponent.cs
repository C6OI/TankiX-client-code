using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class ClientTimeComponent : SharedChangeableComponent {
        float pingClientRealTime;
        long pingServerTime;

        public long PingServerTime {
            get => pingServerTime;
            set {
                pingServerTime = value;
                OnChange();
            }
        }

        public float PingClientRealTime {
            get => pingClientRealTime;
            set {
                pingClientRealTime = value;
                OnChange();
            }
        }
    }
}