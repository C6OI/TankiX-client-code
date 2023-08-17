using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class PreloadAllResourcesScreenSystem : ECSSystem {
        static readonly float WAIT_TIME_BEFORE_SKIP_BUTTON_ENABLE;

        [OnEventFire]
        public void Localize(NodeAddedEvent e, SkipButtonTextNode button, LoadingScreenNode screen) {
            button.textMapping.Text = screen.loadScreenLocalizedText.SkipLoadingButton;
            screen.textMapping.Text = screen.loadScreenLocalizedText.NetworkLoadText;
        }

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
            } else {
                screen.textMapping.Text = screen.loadScreenLocalizedText.LoadFromDiskText;
            }
        }

        [OnEventFire]
        public void SkipPreload(ButtonClickEvent e, SingleNode<SkipLoadButtonComponent> button,
            [JoinAll] LoadingScreenNode screen, [JoinAll] SingleNode<PreloadAllResourcesComponent> preload) {
            screen.loadBundlesTask.TrackedBundles.Clear();
            preload.Entity.RemoveComponent<PreloadAllResourcesComponent>();
        }

        public class LoadingScreenNode : Node {
            public LoadBundlesTaskComponent loadBundlesTask;

            public LoadScreenLocalizedTextComponent loadScreenLocalizedText;
            public PreloadAllResourcesScreenComponent preloadAllResourcesScreen;

            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;

            public TextMappingComponent textMapping;
        }

        public class SkipButtonTextNode : Node {
            public SkipLoadButtonComponent skipLoadButton;

            public TextMappingComponent textMapping;
        }
    }
}