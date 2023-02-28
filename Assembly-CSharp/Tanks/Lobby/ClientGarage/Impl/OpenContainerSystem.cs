using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class OpenContainerSystem : ECSSystem {
        [OnEventFire]
        public void OpenContainer(OpenSelectedContainerEvent e, ItemsContainerItemNode containerNode) {
            ScheduleEvent(new OpenContainerEvent(), containerNode);
        }

        [OnEventFire]
        public void OpenContainer(OpenSelectedContainerEvent e, GamePlayChestItemNode containerNode) {
            ScheduleEvent(new OpenContainerEvent {
                Amount = containerNode.userItemCounter.Count
            }, containerNode);
        }

        public class ItemsContainerItemNode : Node {
            public ContainerMarkerComponent containerMarker;
            public ItemsContainerItemComponent itemsContainerItem;

            public UserItemComponent userItem;
        }

        public class GamePlayChestItemNode : Node {
            public ContainerMarkerComponent containerMarker;
            public GameplayChestItemComponent gameplayChestItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }
    }
}