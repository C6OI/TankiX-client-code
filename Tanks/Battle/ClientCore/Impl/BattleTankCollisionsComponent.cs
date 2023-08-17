using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(6549840349742289518L)]
    [Shared]
    public class BattleTankCollisionsComponent : Component {
        public long SemiActiveCollisionsPhase { get; set; }
    }
}