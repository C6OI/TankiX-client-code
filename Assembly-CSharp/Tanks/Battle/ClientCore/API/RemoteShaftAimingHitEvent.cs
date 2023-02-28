using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(4743444303755604700L)]
    public class RemoteShaftAimingHitEvent : RemoteHitEvent {
        public float HitPower { get; set; }
    }
}