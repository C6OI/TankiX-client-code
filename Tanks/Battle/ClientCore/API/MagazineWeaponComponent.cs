using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(4355651182908057733L)]
    public class MagazineWeaponComponent : Component {
        public float ReloadMagazineTimePerSec { get; set; }

        public int MaxCartridgeCount { get; set; }
    }
}