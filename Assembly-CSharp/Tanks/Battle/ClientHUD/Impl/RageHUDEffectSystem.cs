using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class RageHUDEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayRageEffect(TriggerEffectExecuteEvent e, SingleNode<RageEffectComponent> effect, [JoinByTank] TankNode tank) {
            ScheduleEvent<PlayRageHUDEffectEvent>(tank);
        }

        [OnEventFire]
        public void PlayRageHUDEffect(PlayRageHUDEffectEvent e, TankNode tank, [JoinByUser] [Combine] SlotCooldownStateNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud) {
            float cutTime = slot.inventoryCooldownState.CooldownTime / 1000f - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);
            hud.component.CutCooldown(cutTime);
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        [Not(typeof(EffectsFreeSlotComponent))]
        public class SlotCooldownStateNode : Node {
            public InventoryCooldownStateComponent inventoryCooldownState;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class PlayRageHUDEffectEvent : Event { }
    }
}