using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-5407563795844501148L)]
    [Shared]
    public class StreamHitConfigComponent : Component {
        public float LocalCheckPeriod { get; set; }

        public float SendToServerPeriod { get; set; }

        public bool DetectStaticHit { get; set; }
    }
}