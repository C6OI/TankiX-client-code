using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class TabSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void OnTabPressed(UpdateEvent evt, SingleNode<TabListenerComponent> node, [JoinAll] Optional<SingleNode<RoundActiveStateComponent>> round) {
            bool flag = node.Entity.HasComponent<TabPressedComponent>();

            if (InputManager.CheckAction(BattleActions.SHOW_SCORE) && !flag && round.IsPresent()) {
                TabPressedComponent component = new();
                node.Entity.AddComponent(component);
            } else if ((!InputManager.CheckAction(BattleActions.SHOW_SCORE) || !round.IsPresent()) && flag && node.Entity.HasComponent<TabPressedComponent>()) {
                node.Entity.RemoveComponent<TabPressedComponent>();
            }
        }
    }
}