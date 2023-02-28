using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMHUDMessagesComponent : LocalizedControl, Component {
        public string MainMessage { get; set; }

        public string SpectatorMessage { get; set; }
    }
}