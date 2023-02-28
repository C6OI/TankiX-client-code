using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HullBuilderGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void OnNodeAdded(NodeAddedEvent evt, HullGraphicsNode hullGraphics) {
            Entity entity = hullGraphics.Entity;
            HullInstanceComponent hullInstance = hullGraphics.hullInstance;
            GameObject hullInstance2 = hullInstance.HullInstance;
            hullInstance2.GetComponent<EntityBehaviour>().BuildEntity(entity);
            BaseRendererComponent baseRendererComponent = new();
            baseRendererComponent.Renderer = TankBuilderUtil.GetHullRenderer(hullInstance2);
            baseRendererComponent.Mesh = (baseRendererComponent.Renderer as SkinnedMeshRenderer).sharedMesh;
            entity.AddComponent<StartMaterialsComponent>();
            entity.AddComponent(baseRendererComponent);
            TrackRendererComponent trackRendererComponent = new();
            trackRendererComponent.Renderer = baseRendererComponent.Renderer;
            entity.AddComponent(trackRendererComponent);
            ScheduleEvent<ChassisInitEvent>(entity);
        }

        public class HullGraphicsNode : Node {
            public HullInstanceComponent hullInstance;
        }
    }
}