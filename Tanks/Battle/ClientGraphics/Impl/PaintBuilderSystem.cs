using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PaintBuilderSystem : ECSSystem {
        [OnEventFire]
        public void OnNodeAdded(NodeAddedEvent evt, PaintGraphicsNode paintGraphics) {
            Entity entity = paintGraphics.Entity;
            PaintInstanceComponent paintInstance = paintGraphics.paintInstance;
            GameObject paintInstance2 = paintInstance.PaintInstance;
            paintInstance2.GetComponent<EntityBehaviour>().BuildEntity(entity);
        }

        public class PaintGraphicsNode : Node {
            public PaintInstanceComponent paintInstance;
        }
    }
}