using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatFocusCheckEvent : Event {
        public bool InputIsFocused { get; set; }
    }
}