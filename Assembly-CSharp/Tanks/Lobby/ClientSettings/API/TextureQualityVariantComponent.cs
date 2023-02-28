using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class TextureQualityVariantComponent : Component {
        public int Value { get; set; }

        public int MasterTextureLimit { get; set; }
    }
}