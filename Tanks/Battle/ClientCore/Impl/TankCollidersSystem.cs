using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCollidersSystem : ECSSystem {
        [OnEventFire]
        public void DisableCollider(NodeAddedEvent e, SingleNode<TankCollidersUnityComponent> tankColliders) {
            tankColliders.component.GetTankToTankCollider().enabled = false;
            tankColliders.component.GetBoundsCollider().enabled = false;
        }

        [OnEventFire]
        public void PrepareColliders(NodeAddedEvent evt, TankNode tankNode) {
            TankCollidersComponent tankCollidersComponent = new();
            TankCollidersUnityComponent tankCollidersUnity = tankNode.tankCollidersUnity;
            tankCollidersComponent.BoundsCollider = tankCollidersUnity.GetBoundsCollider();
            tankCollidersComponent.TankToTankCollider = tankCollidersUnity.GetTankToTankCollider();
            tankCollidersComponent.VisualTriggerColliders = CollectMeshColliders(tankNode.assembledTank.AssemblyRoot);
            tankCollidersComponent.TargetingColliders = CollectTargetColliders(tankNode.assembledTank.AssemblyRoot);
            tankCollidersComponent.TankToStaticColliders = CollectTankToStaticColliders(tankNode.assembledTank.AssemblyRoot);

            tankCollidersComponent.Extends =
                Quaternion.Inverse(tankCollidersComponent.TankToTankCollider.transform.rotation) *
                tankCollidersComponent.TankToTankCollider.bounds.extents;

            SetCollidersLayer(tankCollidersComponent,
                Layers.INVISIBLE_PHYSICS,
                Layers.INVISIBLE_PHYSICS,
                Layers.INVISIBLE_PHYSICS);

            SetGameObjectLayers(tankCollidersComponent.TankToStaticColliders, Layers.TANK_TO_STATIC);
            tankNode.Entity.AddComponent(tankCollidersComponent);
        }

        [OnEventFire]
        public void SetCollidersForActiveTank(NodeAddedEvent evt, ActiveTankNode tankNode) {
            SetCollidersLayer(tankNode.tankColliders, Layers.TANK_TO_TANK, Layers.TARGET, Layers.TANK_PART_VISUAL_TRIGGER);
            tankNode.tankColliders.TankToTankCollider.enabled = true;
        }

        [OnEventFire]
        public void DisableTankToTankBeforeDisappear(NodeAddedEvent e, DeadTransparentTankNode tank) {
            tank.tankColliders.TankToTankCollider.enabled = false;
            tank.rigidbody.Rigidbody.gameObject.layer = Layers.INVISIBLE_PHYSICS;
        }

        [OnEventFire]
        public void SetupBoundsColliderForRemoteTank(NodeAddedEvent evt, RemoteTankCollidersNode node) {
            node.tankColliders.BoundsCollider.isTrigger = false;
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.REMOTE_TANK_BOUNDS;
        }

        [OnEventComplete]
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteActiveTankCollidersNode node) =>
            node.tankColliders.BoundsCollider.enabled = true;

        [OnEventComplete]
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteDeadTankCollidersNode node) =>
            node.tankColliders.BoundsCollider.enabled = true;

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeAddedEvent evt, SelfSemiActiveTankNode node) =>
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_SEMIACTIVE_TANK_BOUNDS;

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeRemoveEvent evt, SelfSemiActiveTankNode node) =>
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_TANK_BOUNDS;

        [OnEventFire]
        public void SetCollidersForDeadTank(NodeAddedEvent evt, DeadTankNode tankNode) {
            SetCollidersLayer(tankNode.tankColliders,
                Layers.TANK_TO_TANK,
                Layers.DEAD_TARGET,
                Layers.TANK_PART_VISUAL_TRIGGER);

            tankNode.tankColliders.TankToTankCollider.enabled = true;
        }

        List<GameObject> CollectTargetColliders(GameObject root) => (from c in root.GetComponentsInChildren<Collider>(true)
                                                                     select c.gameObject
                                                                     into go
                                                                     where go.name ==
                                                                           TankCollidersUnityComponent
                                                                               .TARGETING_COLLIDER_NAME
                                                                     select go).ToList();

        List<GameObject> CollectMeshColliders(GameObject root) =>
            (from c in root.GetComponentsInChildren<VisualTriggerMarkerComponent>(true)
             select c.gameObject).ToList();

        List<Collider> CollectTankToStaticColliders(GameObject root) =>
            (from go in root.GetComponentsInChildren<Collider>(true)
             where go.transform.parent.gameObject.name == TankCollidersUnityComponent.TANK_TO_STATIC_COLLIDER_NAME
             select go).ToList();

        void SetCollidersLayer(TankCollidersComponent tankColliders, int tankToTankLayer, int targetingLayer,
            int meshColliderLayer) {
            tankColliders.TankToTankCollider.gameObject.layer = tankToTankLayer;
            SetGameObjectLayers(tankColliders.TargetingColliders, targetingLayer);
            SetGameObjectLayers(tankColliders.VisualTriggerColliders, meshColliderLayer);
        }

        void SetGameObjectLayers(IEnumerable<GameObject> colliders, int layer) {
            foreach (GameObject collider in colliders) {
                collider.layer = layer;
            }
        }

        void SetGameObjectLayers(IEnumerable<Collider> colliders, int layer) {
            foreach (Collider collider in colliders) {
                collider.gameObject.layer = layer;
            }
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TankCollidersComponent tankColliders;
        }

        public class DeadTankNode : Node {
            public TankCollidersComponent tankColliders;

            public TankDeadStateComponent tankDeadState;
        }

        public class SelfSemiActiveTankNode : Node {
            public SelfTankComponent selfTank;

            public TankCollidersComponent tankColliders;

            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class RemoteTankCollidersNode : Node {
            public RemoteTankComponent remoteTank;

            public TankCollidersComponent tankColliders;
        }

        public class RemoteActiveTankCollidersNode : Node {
            public RemoteTankComponent remoteTank;

            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;
        }

        public class RemoteDeadTankCollidersNode : Node {
            public RemoteTankComponent remoteTank;

            public TankCollidersComponent tankColliders;

            public TankDeadStateComponent tankDeadState;
        }

        public class DeadTransparentTankNode : Node {
            public RigidbodyComponent rigidbody;

            public TankCollidersComponent tankColliders;
            public TankDeadStateComponent tankDeadState;

            public TransparencyTransitionComponent transparencyTransition;
        }
    }
}