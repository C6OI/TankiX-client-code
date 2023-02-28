using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void CreateEffect(NodeAddedEvent e, IsisRayEffectInitNode node) {
            GameObject gameObject = Object.Instantiate(node.isisGraphics.RayPrefab);
            CustomRenderQueue.SetQueue(gameObject, 3150);
            UnityUtil.InheritAndEmplace(gameObject.transform, node.muzzlePoint.Current);
            node.isisGraphics.Ray = gameObject.GetComponent<IsisRayEffectBehaviour>();
            node.isisGraphics.Ray.Init();
            node.Entity.AddComponent<IsisGraphicsReadyComponent>();
        }

        [OnEventFire]
        public void ShowEffect(NodeAddedEvent e, WorkingEffectNode node) {
            node.isisGraphics.Ray.Show();
            node.Entity.AddComponent<IsisRayEffectShownComponent>();
        }

        [OnEventFire]
        public void HideEffect(NodeRemoveEvent e, WorkingEffectNode node) {
            node.isisGraphics.Ray.Hide();
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.Entity.RemoveComponent<IsisRayEffectShownComponent>();
        }

        [OnEventFire]
        public void UpdateIsisRayMode(UpdateIsisRayModeEvent evt, SingleNode<TeamComponent> weaponTeam, SingleNode<IsisGraphicsComponent> effectNode, TankNode tank) {
            if (weaponTeam.Entity.Id == tank.teamGroup.Key) {
                effectNode.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
                effectNode.component.Ray.EnableTargetForHealing();
            } else {
                effectNode.Entity.AddComponentIfAbsent<IsisGraphicsDamagingStateComponent>();
                effectNode.component.Ray.EnableTargetForDamaging();
            }
        }

        [OnEventFire]
        public void EnableTarget(NodeAddedEvent e, TargetEffectNode node, [Context] [JoinByBattle] SingleNode<DMComponent> dm) {
            node.Entity.AddComponentIfAbsent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.EnableTargetForDamaging();
            UpdateRayEffectUpdateEvent updateRayEffectUpdateEvent = new();

            updateRayEffectUpdateEvent.speedMultipliers = new float[3] {
                float.PositiveInfinity,
                float.PositiveInfinity,
                float.PositiveInfinity
            };

            NewEvent(updateRayEffectUpdateEvent).Attach(node.streamHit.TankHit.Entity).Attach(node).Schedule();
        }

        [OnEventFire]
        public void EnableTarget(NodeAddedEvent e, [Combine] TargetEffectNode node, [Context] [JoinByTeam] TeamNode team) {
            StreamHitComponent streamHit = node.streamHit;

            NewEvent<UpdateIsisRayModeEvent>().Attach(team).Attach(streamHit.TankHit.Entity).Attach(node)
                .Schedule();

            UpdateRayEffectUpdateEvent updateRayEffectUpdateEvent = new();

            updateRayEffectUpdateEvent.speedMultipliers = new float[3] {
                float.PositiveInfinity,
                float.PositiveInfinity,
                float.PositiveInfinity
            };

            NewEvent(updateRayEffectUpdateEvent).Attach(node).Attach(streamHit.TankHit.Entity).Schedule();
        }

        [OnEventFire]
        public void DisableTarget(NodeRemoveEvent e, DisableEffectNode node) {
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.DisableTarget();
        }

        [OnEventFire]
        public void DisableTarget(NodeRemoveEvent e, TankActiveStateNode activeTank, [JoinByTank] DisableEffectNode node) {
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.DisableTarget();
        }

        [OnEventComplete]
        public void ResendUpdateWithTarget(UpdateEvent e, TargetEffectNode node) {
            UpdateRayEffectUpdateEvent updateRayEffectUpdateEvent = new();
            updateRayEffectUpdateEvent.speedMultipliers = new float[3] { 1f, 2f, 1f };
            updateRayEffectUpdateEvent.bezierPointsRandomness = new float[3] { 0f, 4f, 1f };
            NewEvent(updateRayEffectUpdateEvent).Attach(node).Attach(node.streamHit.TankHit.Entity).Schedule();
        }

        [OnEventFire]
        public void UpdateEffectWithTarget(UpdateRayEffectUpdateEvent e, TargetEffectNode node, SingleNode<TankVisualRootComponent> targetTank, [JoinAll] CameraNode cameraNode) {
            node.isisGraphics.Ray.UpdateTargetPosition(targetTank.component.transform, node.streamHit.TankHit.LocalHitPoint, e.speedMultipliers, e.bezierPointsRandomness);
        }

        public class IsisRayEffectInitNode : Node {
            public IsisGraphicsComponent isisGraphics;

            public MuzzlePointComponent muzzlePoint;
        }

        public class WorkingEffectNode : IsisRayEffectInitNode {
            public IsisGraphicsReadyComponent isisGraphicsReady;

            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class DisableEffectNode : WorkingEffectNode {
            public StreamHitComponent streamHit;
        }

        public class TargetEffectNode : DisableEffectNode {
            public IsisRayEffectShownComponent isisRayEffectShown;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class TeamNode : Node {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }

        public class TankNode : Node {
            public TankComponent tank;
            public TeamGroupComponent teamGroup;
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        public class TankActiveStateNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}