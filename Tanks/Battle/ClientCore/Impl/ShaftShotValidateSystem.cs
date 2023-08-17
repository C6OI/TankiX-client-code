using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftShotValidateSystem : ECSSystem {
        [OnEventFire]
        public void SetMask(NodeAddedEvent evt, ShaftIdleNode weapon) {
            weapon.shotValidate.BlockValidateMask = LayerMasks.STATIC;
            weapon.shotValidate.UnderGroundValidateMask = LayerMasks.STATIC;
            weapon.shotValidate.RaycastExclusionGameObjects = null;
        }

        [OnEventFire]
        public void SetMask(NodeAddedEvent evt, ShaftWaitingNode weapon, [JoinByTank] TankCollidersNode tank) {
            weapon.shotValidate.BlockValidateMask = LayerMasks.VISUAL_TARGETING;
            weapon.shotValidate.UnderGroundValidateMask = LayerMasks.VISUAL_STATIC;
            weapon.shotValidate.RaycastExclusionGameObjects = tank.tankColliders.VisualTriggerColliders.ToArray();
        }

        public class ShaftWaitingNode : Node {
            public ShaftStateControllerComponent shaftStateController;

            public ShaftWaitingStateComponent shaftWaitingState;

            public ShotValidateComponent shotValidate;

            public TankGroupComponent tankGroup;
        }

        public class ShaftIdleNode : Node {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStateControllerComponent shaftStateController;

            public ShotValidateComponent shotValidate;

            public TankGroupComponent tankGroup;
        }

        public class TankCollidersNode : Node {
            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }
    }
}