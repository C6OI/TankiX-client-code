using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class UILogComponent : Component {
        public UILogComponent(UILog uiLog) => UILog = uiLog;

        public UILog UILog { get; set; }
    }
}