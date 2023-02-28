using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1485846320654L)]
    public class SlotUserItemInfoComponent : Component {
        public Slot Slot { get; set; }

        public ModuleBehaviourType ModuleBehaviourType { get; set; }

        public int UpgradeLevel { get; set; }
    }
}