using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlagVisualDropSystem : ECSSystem {
        [OnEventFire]
        public void DropFlag(NodeAddedEvent e, FlagNode flag) {
            Transform transform = flag.flagInstance.FlagInstance.transform;
            transform.parent = null;
            transform.position = flag.flagPosition.Position;
            transform.rotation = Quaternion.identity;
        }

        public class FlagNode : Node {
            public FlagGroundedStateComponent flagGroundedState;

            public FlagInstanceComponent flagInstance;
            public FlagPositionComponent flagPosition;
        }
    }
}