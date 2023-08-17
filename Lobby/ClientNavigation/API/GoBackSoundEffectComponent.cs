using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class GoBackSoundEffectComponent : UISoundEffectController, Component {
        public override string HandlerName => "Cancel";
    }
}