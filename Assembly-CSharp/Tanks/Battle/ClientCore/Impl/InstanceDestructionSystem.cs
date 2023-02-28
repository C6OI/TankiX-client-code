using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InstanceDestructionSystem : ECSSystem {
        [OnEventFire]
        public void OnNodeRemoveEvent(NodeRemoveEvent evt, InstanceDestructionNode instanceDestruction) {
            Object.Destroy(instanceDestruction.instanceDestruction.GameObject);
        }

        public class InstanceDestructionNode : Node {
            public InstanceDestructionComponent instanceDestruction;
        }
    }
}