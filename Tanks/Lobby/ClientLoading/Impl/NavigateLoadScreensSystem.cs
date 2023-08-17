using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class NavigateLoadScreensSystem : ECSSystem {
        [OnEventFire]
        public void ShowLobbyLoadScreen(ShowLobbyScreenEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<ShowScreenNoAnimationEvent<LobbyLoadScreenComponent>>(user);
            CreateEntity<WarmupResourcesTemplate>("service/warmupresources");
        }

        [OnEventFire]
        public void StartPreload(NodeAddedEvent e, SingleNode<LoadProgressTaskCompleteComponent> loadTaskNode,
            SingleNode<UserReadyForLobbyComponent> user, [JoinAll] SingleNode<LobbyLoadScreenComponent> screen) =>
            user.Entity.AddComponent<PreloadAllResourcesComponent>();

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(NodeAddedEvent e, SingleNode<PreloadAllResourcesComponent> node) =>
            NewEvent<ShowPreloadScreenDelayedEvent>().Attach(node).ScheduleDelayed(0.5f);

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(ShowPreloadScreenDelayedEvent e,
            SingleNode<PreloadAllResourcesComponent> node, [JoinAll] ICollection<SingleNode<PreloadComponent>> preloads) {
            if (preloads.Count > 0) {
                ScheduleEvent<ShowScreenNoAnimationEvent<PreloadAllResourcesScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void ShowBattleLoadScreen(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> battleUser) =>
            ScheduleEvent<ShowScreenNoAnimationEvent<BattleLoadScreenComponent>>(battleUser);

        public class ShowPreloadScreenDelayedEvent : Event { }
    }
}