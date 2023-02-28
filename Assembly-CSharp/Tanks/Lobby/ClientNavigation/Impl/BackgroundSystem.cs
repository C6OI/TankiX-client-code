using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class BackgroundSystem : ECSSystem {
        [OnEventFire]
        public void MoveBackgroundOnInit(NodeAddedEvent e, SingleNode<BackgroundComponent> background, SingleNode<ScreensLayerComponent> screensLayer) {
            background.component.transform.SetParent(screensLayer.component.transform, false);
            background.component.transform.SetAsFirstSibling();
        }

        [OnEventFire]
        public void ShowBackground(NodeAddedEvent e, ActiveScreenNode screen, [JoinAll] SingleNode<BackgroundComponent> background) {
            background.component.Show();
        }

        [OnEventFire]
        public void HideBackground(NodeAddedEvent e, HidingScreenNode screen, [JoinAll] SingleNode<BackgroundComponent> background) {
            background.component.Hide();
        }

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ShowBackgroundComponent showBackground;
        }

        public class HidingScreenNode : Node {
            public ScreenHidingComponent screenHiding;
            public ShowBackgroundComponent showBackground;
        }
    }
}