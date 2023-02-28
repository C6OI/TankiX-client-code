using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class VSyncSettingVariantComponent : Component {
        public int Value { get; set; }
    }
}