using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(2388237143993578319L)]
    public class MagazineStorageComponent : Component {
        public int CurrentCartridgeCount { get; set; }
    }
}