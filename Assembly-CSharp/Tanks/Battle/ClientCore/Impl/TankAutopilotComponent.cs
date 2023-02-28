using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1450950140134L)]
    [Shared]
    public class TankAutopilotComponent : Component {
        [ProtocolOptional] public Entity Session { get; set; }

        public int Version { get; set; }
    }
}