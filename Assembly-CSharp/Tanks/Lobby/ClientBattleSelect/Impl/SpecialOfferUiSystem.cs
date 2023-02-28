using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferUiSystem : ECSSystem {
        [OnEventFire]
        public void ClickOpenContainerButton(ButtonClickEvent e, SingleNode<SpecialOfferOpenContainerButton> button, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(button.component.containerId);

            BattleResultAwardsScreenSystem.GamePlayChestItemNode gamePlayChestItemNode =
                Select<BattleResultAwardsScreenSystem.GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault();

            if (gamePlayChestItemNode != null && gamePlayChestItemNode.userItemCounter.Count != 0) {
                ScheduleEvent(new OpenContainerEvent {
                    Amount = button.component.quantity
                }, gamePlayChestItemNode);

                button.component.onOpen();
            }
        }
    }
}