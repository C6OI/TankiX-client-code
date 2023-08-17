using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics {
    public class BonusRegionShowSystem : ECSSystem {
        [OnEventFire]
        public void CreateBonusRegionMaterialComponent(NodeAddedEvent evt,
            SingleNode<BonusRegionInstanceComponent> bonusRegion) => bonusRegion.Entity.AddComponent(
            new MaterialComponent(MaterialAlphaUtils.GetMaterial(bonusRegion.component.BonusRegionInstance)));

        [OnEventFire]
        public void UpdateRegionOpacityByDistance(TimeUpdateEvent e, TankNode tank,
            [Combine] [JoinAll] VisibleBonusRegionNode region,
            [JoinAll] SingleNode<BonusRegionClientConfigComponent> configNode,
            [JoinAll] SingleNode<RoundActiveStateComponent> round) {
            BonusRegionClientConfigComponent component = configNode.component;

            float num = Vector3.Distance(tank.tankColliders.BoundsCollider.transform.position,
                region.spatialGeometry.Position);

            region.opacityBonusRegion.Opacity =
                Mathf.Clamp01(1f -
                              (num - component.maxOpacityRadius) /
                              (component.minOpacityRadius - component.maxOpacityRadius));
        }

        [OnEventFire]
        public void SetRegionTransparent(NodeAddedEvent e, InvisibleBonusRegionNode region) =>
            region.opacityBonusRegion.Opacity = 0f;

        [OnEventFire]
        public void SetRegionTransparent(NodeRemoveEvent e, VisibleBonusRegionNode region) =>
            region.opacityBonusRegion.Opacity = 0f;

        [OnEventComplete]
        public void UpdateRegionOpacity(TimeUpdateEvent e, BonusRegionNode node,
            [JoinAll] SingleNode<BonusRegionClientConfigComponent> configNode) {
            Material material = node.material.Material;
            float alpha = material.GetAlpha();
            float num = e.DeltaTime * configNode.component.opacityChangingSpeed;
            float alpha2 = Mathf.Clamp(node.opacityBonusRegion.Opacity, alpha - num, alpha + num);
            material.SetAlpha(alpha2);
        }

        public class TankNode : Node {
            public SelfTankComponent selfTank;

            public TankCollidersComponent tankColliders;

            public TankMovableComponent tankMovable;
        }

        public class BonusRegionNode : Node {
            public BonusRegionComponent bonusRegion;

            public BonusRegionInstanceComponent bonusRegionInstance;

            public MaterialComponent material;

            public OpacityBonusRegionComponent opacityBonusRegion;

            public SpatialGeometryComponent spatialGeometry;
        }

        public class VisibleBonusRegionNode : Node {
            public BonusRegionComponent bonusRegion;

            public OpacityBonusRegionComponent opacityBonusRegion;

            public SpatialGeometryComponent spatialGeometry;

            public VisibleBonusRegionComponent visibleBonusRegion;
        }

        [Not(typeof(VisibleBonusRegionComponent))]
        public class InvisibleBonusRegionNode : Node {
            public BonusRegionComponent bonusRegion;

            public OpacityBonusRegionComponent opacityBonusRegion;

            public SpatialGeometryComponent spatialGeometry;
        }
    }
}