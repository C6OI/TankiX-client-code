using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientControls.Impl {
    public class CommonControlsSystem : ECSSystem {
        [OnEventFire]
        public void SetLocalziedText(NodeAddedEvent e, LocalizedTextNode node) =>
            node.textMapping.Text = node.localizedText.Text;

        public class LocalizedTextNode : Node {
            public LocalizedTextComponent localizedText;

            public TextMappingComponent textMapping;
        }
    }
}