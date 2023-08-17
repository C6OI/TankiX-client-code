using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class BattleLoadScreenSystem : ECSSystem {
        [OnEventFire]
        public void Localize(NodeAddedEvent e, LoadingScreenNode screen) =>
            screen.textMapping.Text = screen.loadScreenLocalizedText.LoadFromDiskText;

        [OnEventFire]
        public void UpdateText(UpdateEvent e, LoadingScreenNode screen,
            [JoinAll] SingleNode<LoadBundlesTaskComponent> loadBundlesTaskNode) {
            LoadBundlesTaskComponent component = loadBundlesTaskNode.component;

            if (component.BytesToLoad - component.BytesLoaded < 5242880) {
                screen.textMapping.Text = screen.loadScreenLocalizedText.InitializationText;
            } else if (component.MBytesToLoadFromNetwork > 0 &&
                       component.MBytesLoadedFromNetwork < component.MBytesToLoadFromNetwork) {
                screen.textMapping.Text = string.Format("{0}. {1} MB / {2} MB",
                    screen.loadScreenLocalizedText.NetworkLoadText,
                    component.MBytesLoadedFromNetwork,
                    component.MBytesToLoadFromNetwork);
            } else {
                screen.textMapping.Text = screen.loadScreenLocalizedText.LoadFromDiskText;
            }
        }

        public class LoadingScreenNode : Node {
            public LoadScreenLocalizedTextComponent loadScreenLocalizedText;
            public BattleLoadScreenSystem lobbyLoadScreen;

            public TextMappingComponent textMapping;
        }
    }
}