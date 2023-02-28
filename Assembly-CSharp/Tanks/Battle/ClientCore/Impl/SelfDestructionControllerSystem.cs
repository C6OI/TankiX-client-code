using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SelfDestructionControllerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void OnUpdate(UpdateEvent evt, SuicideControllerNode node) {
            if (InputManager.CheckAction(SuicideActions.SUICIDE)) {
                node.Entity.AddComponent<SelfDestructionComponent>();
            }
        }

        [Not(typeof(SelfDestructionComponent))]
        public class SuicideControllerNode : Node {
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }
    }
}