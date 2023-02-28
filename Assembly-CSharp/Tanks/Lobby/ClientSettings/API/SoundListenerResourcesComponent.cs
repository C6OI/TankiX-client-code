using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class SoundListenerResourcesComponent : Component {
        public SoundListenerResourcesComponent(SoundListenerResourcesBehaviour resources) => Resources = resources;

        public SoundListenerResourcesBehaviour Resources { get; set; }
    }
}