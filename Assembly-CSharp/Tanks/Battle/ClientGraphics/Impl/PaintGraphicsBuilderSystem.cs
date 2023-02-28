using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PaintGraphicsBuilderSystem : ECSSystem {
        [OnEventFire]
        public void OnNodeAdded(NodeAddedEvent evt, PaintGraphicsNode paintGraphics) {
            Entity entity = paintGraphics.Entity;
            TankPartPaintInstanceComponent tankPartPaintInstance = paintGraphics.tankPartPaintInstance;
            GameObject paintInstance = tankPartPaintInstance.PaintInstance;
            paintInstance.GetComponent<EntityBehaviour>().BuildEntity(entity);
        }

        public class PaintGraphicsNode : Node {
            public TankPartPaintInstanceComponent tankPartPaintInstance;
        }
    }
}