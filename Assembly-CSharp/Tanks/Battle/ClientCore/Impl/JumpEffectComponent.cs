using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1538454650646L)]
    public class JumpEffectComponent : Component {
        public float BaseImpact { get; set; }

        public float GravityPenalty { get; set; }

        public bool ScaleByMass { get; set; }

        public bool AlwaysUp { get; set; }

        public long FlyComponentDelayInMs { get; set; }
    }
}