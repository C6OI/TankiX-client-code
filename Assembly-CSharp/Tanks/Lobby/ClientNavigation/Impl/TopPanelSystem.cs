using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class TopPanelSystem : ECSSystem {
        [OnEventFire]
        public void ActivateTopPanelItems(NodeAddedEvent e, ActiveScreenNode screen, SingleNode<CommonScreenElementsComponent> topPanel) {
            topPanel.component.ActivateItems(screen.screen.VisibleCommonScreenElements);
        }

        [OnEventFire]
        public void GoBack(ButtonClickEvent e, SingleNode<BackButtonComponent> button) {
            if (!button.component.Disabled) {
                ScheduleEvent<GoBackRequestEvent>(button.Entity);
            }
        }

        [OnEventFire]
        public void EnableBackButtonWhenLoadFail(EnterBattleFailedEvent e, Node anyNode, [JoinAll] SingleNode<BackButtonComponent> backButton) {
            backButton.component.Disabled = false;
        }

        [OnEventFire]
        public void DisableBackButtonWhenLoad(EnterBattleAttemptEvent e, Node anyNode, [JoinAll] SingleNode<BackButtonComponent> backButton) {
            backButton.component.Disabled = true;
        }

        [OnEventFire]
        public void UpdateBackgroundVisibility(NodeAddedEvent e, ScreenWithTopPanelConstructorNode screen, TopPanelNode topPanel) {
            GameObject gameObject = topPanel.topPanel.background.gameObject;
            gameObject.SetActive(screen.topPanelConstructor.ShowBackground);
        }

        [OnEventFire]
        public void UpdateHeaderVisibility(NodeAddedEvent e, ScreenWithTopPanelConstructorNode screen, TopPanelNode topPanel) {
            GameObject gameObject = topPanel.topPanel.screenHeader.gameObject;
            gameObject.SetActive(screen.topPanelConstructor.ShowHeader);
        }

        [OnEventComplete]
        public void SendHeaderTextEvent(NodeAddedEvent e, TopPanelNode topPanel, SingleNode<ScreenHeaderTextComponent> screenHeader,
            [Context] [JoinByScreen] SingleNode<ActiveScreenComponent> screen) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Animated(screenHeader.component.HeaderText);
            ScheduleEvent(setScreenHeaderEvent, screenHeader.Entity);
        }

        [OnEventFire]
        public void SetHeaderText(SetScreenHeaderEvent e, Node any, [JoinAll] TopPanelNode topPanel) {
            if (e.Animate) {
                topPanel.topPanel.SetHeaderText(e.Header);
            } else {
                topPanel.topPanel.SetHeaderTextImmediately(e.Header);
            }
        }

        [OnEventFire]
        public void ShowHeaderAnimation(GoBackEvent e, Node any, [JoinAll] TopPanelNode topPanel) {
            topPanel.topPanel.screenHeader.SetTrigger("back");
        }

        [OnEventFire]
        public void ShowHeaderAnimation(ShowScreenEvent e, Node any, [JoinAll] TopPanelNode topPanel) {
            if (topPanel.topPanel.HasHeader) {
                topPanel.topPanel.screenHeader.SetTrigger("forward");
            }
        }

        public class TopPanelNode : Node {
            public TopPanelComponent topPanel;
        }

        public class NavigationNode : Node {
            public CurrentScreenComponent currentScreen;
        }

        public class ScreenWithTopPanelConstructorNode : Node {
            public ScreenComponent screen;

            public TopPanelConstructorComponent topPanelConstructor;
        }

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }
    }
}