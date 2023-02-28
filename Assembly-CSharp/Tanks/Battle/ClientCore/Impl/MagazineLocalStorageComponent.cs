using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class MagazineLocalStorageComponent : Component {
        public MagazineLocalStorageComponent() { }

        public MagazineLocalStorageComponent(int currentCartridgeCount) => CurrentCartridgeCount = currentCartridgeCount;

        public int CurrentCartridgeCount { get; set; }
    }
}