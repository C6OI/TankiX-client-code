using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class BattleChatLocalizedStringsComponent : Component {
        public string TeamChatInputHint { get; set; }

        public string GeneralChatInputHint { get; set; }
    }
}