using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-6274985110858845212L)]
    public class StreamHitComponent : Component {
        [ProtocolOptional] public HitTarget TankHit { get; set; }

        [ProtocolOptional] public StaticHit StaticHit { get; set; }
    }
}