using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankCommonBuilderSystem : ECSSystem {
        [OnEventComplete]
        public void OnNodeAdded(NodeAddedEvent evt, TankCommonGraphicsNode tankCommonGraphics) {
            Entity entity = tankCommonGraphics.Entity;
            TankCommonInstanceComponent tankCommonInstance = tankCommonGraphics.tankCommonInstance;
            GameObject tankCommonInstance2 = tankCommonInstance.TankCommonInstance;
            tankCommonInstance2.GetComponent<EntityBehaviour>().BuildEntity(entity);
        }

        public class TankCommonGraphicsNode : Node {
            public TankCommonInstanceComponent tankCommonInstance;
        }
    }
}