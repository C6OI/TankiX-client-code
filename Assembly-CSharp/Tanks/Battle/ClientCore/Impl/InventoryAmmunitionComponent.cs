using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636383014039871905L)]
    public class InventoryAmmunitionComponent : Component {
        public int MaxCount { get; set; }

        public int CurrentCount { get; set; }
    }
}