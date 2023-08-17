using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class LobbyLoadScreenSystem : ECSSystem {
        [OnEventFire]
        public void Localize(NodeAddedEvent e, LoadingScreenNode screen) =>
            screen.textMapping.Text = screen.loadScreenLocalizedText.NetworkLoadText;

        [OnEventFire]
        public void UpdateText(UpdateEvent e, LoadingScreenNode screen,
            [JoinAll] SingleNode<LoadBundlesTaskComponent> loadBundlesTaskNode) {
            LoadBundlesTaskComponent component = loadBundlesTaskNode.component;
            int mBytesLoadedFromNetwork = component.MBytesLoadedFromNetwork;
            int mBytesToLoadFromNetwork = component.MBytesToLoadFromNetwork;

            if (mBytesToLoadFromNetwork > 0 && mBytesLoadedFromNetwork < mBytesToLoadFromNetwork) {
                screen.textMapping.Text = string.Format("{0} \n{1} MB / {2} MB",
                    screen.loadScreenLocalizedText.NetworkLoadText,
                    mBytesLoadedFromNetwork,
                    mBytesToLoadFromNetwork);
            } else if (component.BytesToLoad > component.BytesLoaded) {
                screen.textMapping.Text = screen.loadScreenLocalizedText.LoadFromDiskText;
            } else {
                screen.textMapping.Text = screen.loadScreenLocalizedText.InitializationText;
            }
        }

        public class LoadingScreenNode : Node {
            public LoadScreenLocalizedTextComponent loadScreenLocalizedText;
            public LobbyLoadScreenComponent lobbyLoadScreen;

            public TextMappingComponent textMapping;
        }
    }
}