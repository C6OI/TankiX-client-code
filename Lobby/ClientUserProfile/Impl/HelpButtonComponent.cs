using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class HelpButtonComponent : LocalizedControl, Component {
        public string Url { get; set; }
    }
}