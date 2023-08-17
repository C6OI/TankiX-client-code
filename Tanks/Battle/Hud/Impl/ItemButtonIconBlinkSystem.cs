using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonIconBlinkSystem : ECSSystem {
        [OnEventFire]
        public void StartBlinking(NodeAddedEvent e, EffectNode effect, [JoinBySupply] ItemButtonNode button) =>
            button.itemButtonBlinker.blinker.StartBlink();

        [OnEventFire]
        public void StopBlinking(NodeRemoveEvent e, EffectNode effect, [JoinBySupply] ItemButtonNode button) =>
            button.itemButtonBlinker.blinker.StopBlink();

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonBlinkerComponent itemButtonBlinker;

            public SupplyGroupComponent supplyGroup;
        }

        public class ActivatedItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ItemButtonActivatedStateComponent itemButtonActivatedState;

            public ItemButtonBlinkerComponent itemButtonBlinker;
        }

        public class EffectNode : Node {
            public EffectComponent effect;

            public SupplyGroupComponent supplyGroup;
        }
    }
}