using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636319924231605265L)]
    public class ModuleCardsCompositionComponent : Component {
        public ModulePrice CraftPrice { get; set; }

        public List<ModulePrice> UpgradePrices { get; set; }
    }
}