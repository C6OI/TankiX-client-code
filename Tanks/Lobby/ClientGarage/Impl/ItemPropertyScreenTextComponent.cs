using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemPropertyScreenTextComponent : Component {
        public string PropertyWeaponText { get; set; }

        public string PropertyTankText { get; set; }

        public string PropertySupplyText { get; set; }
    }
}