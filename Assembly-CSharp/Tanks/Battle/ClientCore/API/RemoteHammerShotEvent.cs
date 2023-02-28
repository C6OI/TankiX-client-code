using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-8245726943400840523L)]
    public class RemoteHammerShotEvent : RemoteShotEvent {
        public int RandomSeed { get; set; }
    }
}