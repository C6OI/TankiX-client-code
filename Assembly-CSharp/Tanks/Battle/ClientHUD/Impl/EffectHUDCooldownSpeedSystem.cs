using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class EffectHUDCooldownSpeedSystem : ECSSystem {
        [OnEventFire]
        public void PlayChangeCooldownSpeedHUDAnimation(BattleUserInventoryCooldownSpeedChangedEvent e, SingleNode<BattleUserInventoryCooldownSpeedComponent> battleUser,
            [JoinByUser] [Combine] HUDNodes.Modules.SlotWithModuleNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud) {
            hud.component.SetCooldownCoeff(battleUser.component.SpeedCoeff);
            hud.component.isRage = !hud.component.isRage;
        }

        [OnEventFire]
        public void ChangeCooldownSpeedHUDAnimation(BattleUserInventoryCooldownSpeedChangedEvent e, SingleNode<BattleUserInventoryCooldownSpeedComponent> battleUser,
            [JoinByUser] [Combine] SlotCooldownStateNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud) {
            float time = slot.inventoryCooldownState.CooldownTime / 1000f - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);
            hud.component.ChangeCooldown(time, battleUser.component.SpeedCoeff, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
        }

        [Not(typeof(EffectsFreeSlotComponent))]
        public class SlotCooldownStateNode : Node {
            public InventoryCooldownStateComponent inventoryCooldownState;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }
    }
}