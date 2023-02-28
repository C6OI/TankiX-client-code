using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class HelpButtonComponent : LocalizedControl, Component {
        public string Url { get; set; }
    }
}