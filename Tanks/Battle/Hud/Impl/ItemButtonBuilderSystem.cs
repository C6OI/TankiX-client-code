using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonBuilderSystem : ECSSystem {
        [OnEventFire]
        public void SupplyItemButtonCreated(NodeAddedEvent e, SingleNode<ItemButtonComponent> button) {
            ItemButtonESMComponent itemButtonESMComponent = new();
            button.Entity.AddComponent(itemButtonESMComponent);
            EntityStateMachine esm = itemButtonESMComponent.Esm;
            esm.AddState<ItemButtonStates.ItemButtonEnabledStateNode>();
            esm.AddState<ItemButtonStates.ItemButtonDisabledStateNode>();
            esm.AddState<ItemButtonStates.ItemButtonActivatedStateNode>();
            esm.AddState<ItemButtonStates.ItemButtonCooldownStateNode>();
        }
    }
}