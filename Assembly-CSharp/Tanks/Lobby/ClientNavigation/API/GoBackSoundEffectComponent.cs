using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class GoBackSoundEffectComponent : UISoundEffectController, Component {
        public override string HandlerName => "Cancel";
    }
}