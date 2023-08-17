using System;
using System.Collections.Generic;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lobby.ClientNavigation.Impl {
    public class NavigationSystem : ECSSystem {
        [OnEventFire]
        public void CreateGroup(NodeAddedEvent e, SingleNode<ScreenComponent> node) =>
            node.Entity.CreateGroup<ScreenGroupComponent>();

        [OnEventFire]
        public void RegisterScreens(NodeAddedEvent e, SingleNode<ScreensRegistryComponent> navigationNode,
            [Combine] [Context] SingleNode<ScreensBundleComponent> screensBundleNode,
            SingleNode<ScreensLayerComponent> layerNode) {
            foreach (ScreenComponent screen in screensBundleNode.component.Screens) {
                GameObject gameObject = screen.gameObject;
                navigationNode.component.screens.Add(gameObject);
                screen.transform.SetParent(layerNode.component.screensLayer, false);
            }

            if (screensBundleNode.component.Dialogs != null) {
                screensBundleNode.component.Dialogs.transform.SetParent(layerNode.component.dialogsLayer, false);
            }

            Object.Destroy(screensBundleNode.component.gameObject);
            ScheduleEvent<ScreensLoadedEvent>(navigationNode);
        }

        [OnEventComplete]
        public void TryShowRequestedScreenAfterScreensLoad(ScreensLoadedEvent e, NavigationRequestNode navigationNode) =>
            TryShowScreen(navigationNode);

        [OnEventFire]
        public void TryShowRequestedScreen(NodeAddedEvent e, NavigationRequestNode navigationNode) =>
            TryShowScreen(navigationNode);

        [OnEventFire]
        public void RequestShowScreen(ShowScreenEvent e, Node any,
            [JoinAll] SingleNode<ScreensRegistryComponent> navigationNode) {
            RequestShowScreenComponent requestShowScreenComponent = new();
            requestShowScreenComponent.Event = e;
            navigationNode.Entity.AddComponent(requestShowScreenComponent);
        }

        [OnEventFire]
        public void GoBack(GoBackRequestEvent e, Node any, [JoinAll] NavigationNode navigationNode,
            [JoinAll] ScreenNode screen) {
            HistoryComponent history = navigationNode.history;

            if (history.screens.Count <= 0) {
                return;
            }

            ShowScreenData showScreenData = navigationNode.history.screens.Peek();
            ScreensRegistryComponent screensRegistry = navigationNode.screensRegistry;
            GameObject nonActiveScreen = GetNonActiveScreen(showScreenData.ScreenType, screensRegistry);

            if (nonActiveScreen != null) {
                ScheduleEvent<GoBackEvent>(any.Entity);
                navigationNode.history.screens.Pop();
                CurrentScreenComponent currentScreen = navigationNode.currentScreen;

                if (currentScreen.showScreenData.Context != null) {
                    currentScreen.showScreenData.Context.RemoveComponent<ScreenGroupComponent>();
                }

                GameObject activeScreen =
                    GetActiveScreen(currentScreen.showScreenData.ScreenType, navigationNode.screensRegistry);

                currentScreen.screen.RemoveComponent<ActiveScreenComponent>();
                currentScreen.screen.AddComponent<ScreenHidingComponent>();
                currentScreen.showScreenData.FreeContext();
                ActivateShowingScreen(nonActiveScreen, activeScreen, showScreenData, currentScreen);
                NewEvent<PostGoBackEvent>().Attach(navigationNode).ScheduleDelayed(0f);
            }
        }

        [OnEventFire]
        public void SetCurrentScreen(NodeAddedEvent e, SingleNode<ScreenComponent> screenNode,
            [JoinAll] SingleNode<CurrentScreenComponent> currentScreen) {
            CurrentScreenComponent component = currentScreen.component;
            component.screen = screenNode.Entity;
            screenNode.Entity.AddComponent<ActiveScreenComponent>();
        }

        [OnEventFire]
        public void JoinContextToScreen(NodeAddedEvent e, GroupScreenNode screenNode,
            [JoinAll] [Mandatory] SingleNode<CurrentScreenComponent> currentScreen) {
            ShowScreenData showScreenData = currentScreen.component.showScreenData;

            if (showScreenData.Context != null) {
                JoinContext(showScreenData.Context, screenNode.Entity);
            }
        }

        void TryShowScreen(NavigationRequestNode navigationNode) {
            ShowScreenData showScreenData = navigationNode.requestShowScreen.Event.ShowScreenData;
            ScreensRegistryComponent screensRegistry = navigationNode.screensRegistry;
            GameObject nonActiveScreen = GetNonActiveScreen(showScreenData.ScreenType, screensRegistry);

            if (nonActiveScreen != null) {
                navigationNode.Entity.RemoveComponent<RequestShowScreenComponent>();
                CurrentScreenComponent currentScreen = navigationNode.currentScreen;
                HistoryComponent history = navigationNode.history;
                GameObject gameObject = null;

                if (currentScreen.screen != null) {
                    gameObject = GetActiveScreen(currentScreen.showScreenData.ScreenType, navigationNode.screensRegistry);
                    currentScreen.screen.RemoveComponent<ActiveScreenComponent>();
                    currentScreen.screen.AddComponent<ScreenHidingComponent>();

                    if (gameObject.GetComponent<ScreenComponent>().LogInHistory) {
                        ShowScreenData showScreenData2 =
                            currentScreen.showScreenData.InvertAnimationDirection(showScreenData.AnimationDirection);

                        Stack<ShowScreenData> screens = history.screens;

                        if (screens.Count > 0 && screens.Peek().ScreenType == showScreenData2.ScreenType) {
                            screens.Pop();
                        }

                        screens.Push(showScreenData2);

                        if (showScreenData2.Context != null) {
                            showScreenData2.Context.RemoveComponent<ScreenGroupComponent>();
                        }
                    }
                }

                ActivateShowingScreen(nonActiveScreen, gameObject, showScreenData, currentScreen);
            } else if (GetActiveScreen(showScreenData.ScreenType, screensRegistry) != null) {
                navigationNode.Entity.RemoveComponent<RequestShowScreenComponent>();
            }
        }

        [OnEventFire]
        public void ClearHistory(NodeAddedEvent evt, SingleNode<ScreenComponent> screen,
            SingleNode<HistoryComponent> navigation) {
            ScreenHistoryCleaner component = screen.component.GetComponent<ScreenHistoryCleaner>();

            if (!(component == null)) {
                component.ClearHistory(navigation.component.screens);
            }
        }

        void ActivateShowingScreen(GameObject showingScreen, GameObject hidingScreen, ShowScreenData showScreenData,
            CurrentScreenComponent currentScreenComponent) {
            currentScreenComponent.showScreenData = showScreenData;
            showingScreen.GetComponent<EntityBehaviour>().enabled = false;
            showingScreen.SetActive(true);
            showingScreen.GetComponent<EntityBehaviour>().enabled = true;
            PlayAnimation(showingScreen, hidingScreen, showScreenData.AnimationDirection);
        }

        void JoinContext(Entity context, Entity key) {
            if (context.HasComponent<ScreenGroupComponent>()) {
                context.RemoveComponent<ScreenGroupComponent>();
            }

            context.AddComponent(new ScreenGroupComponent(key));
        }

        void PlayAnimation(GameObject showingScreen, GameObject hidingScreen, AnimationDirection animationDirection) {
            if (hidingScreen != null) {
                PlayHideAnimation(hidingScreen.GetComponent<Animator>(), animationDirection);
            }

            PlayShowAnimation(showingScreen.GetComponent<Animator>(), animationDirection);
        }

        GameObject GetNonActiveScreen(Type screenType, ScreensRegistryComponent screens) =>
            GetScreen(screenType, screens, false);

        GameObject GetActiveScreen(Type screenType, ScreensRegistryComponent screens) =>
            GetScreen(screenType, screens, true);

        GameObject GetScreen(Type screenType, ScreensRegistryComponent screens, bool active) {
            foreach (GameObject screen in screens.screens) {
                if (screen.GetComponent(screenType) != null && screen.activeSelf == active) {
                    return screen;
                }
            }

            return null;
        }

        void PlayShowAnimation(Animator screen, AnimationDirection dir) {
            screen.SetInteger("type", (int)dir);
            screen.SetTrigger("show");
        }

        void PlayHideAnimation(Animator screen, AnimationDirection dir) {
            screen.SetInteger("type", (int)dir);
            screen.SetTrigger("hide");
        }

        public class NavigationRequestNode : Node {
            public CurrentScreenComponent currentScreen;

            public HistoryComponent history;

            public RequestShowScreenComponent requestShowScreen;
            public ScreensRegistryComponent screensRegistry;
        }

        public class NavigationNode : Node {
            public CurrentScreenComponent currentScreen;

            public HistoryComponent history;

            public ScreensRegistryComponent screensRegistry;
        }

        [Not(typeof(LockedScreenComponent))]
        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
        }

        public class GroupScreenNode : Node {
            public ScreenComponent screen;

            public ScreenGroupComponent screenGroup;
        }
    }
}