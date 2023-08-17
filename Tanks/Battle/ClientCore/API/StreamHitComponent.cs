using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-6274985110858845212L)]
    [Shared]
    public class StreamHitComponent : Component {
        [ProtocolOptional] public HitTarget TankHit { get; set; }

        [ProtocolOptional] public StaticHit StaticHit { get; set; }
    }
}