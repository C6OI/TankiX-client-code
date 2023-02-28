using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarAmbientSoundControllerComponent : Component {
        public HangarAmbientSoundControllerComponent(HangarAmbientSoundController hangarAmbientSoundController) => HangarAmbientSoundController = hangarAmbientSoundController;

        public HangarAmbientSoundController HangarAmbientSoundController { get; set; }
    }
}