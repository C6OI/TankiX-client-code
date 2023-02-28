using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankCollidersSystem : ECSSystem {
        [OnEventFire]
        public void PrepareColliders(NodeAddedEvent evt, TankNode tankNode) {
            TankCollidersComponent tankCollidersComponent = new();
            TankCollidersUnityComponent tankCollidersUnity = tankNode.tankCollidersUnity;
            tankCollidersComponent.BoundsCollider = tankCollidersUnity.GetBoundsCollider();
            tankCollidersComponent.TankToTankCollider = tankCollidersUnity.GetTankToTankCollider();
            tankCollidersComponent.VisualTriggerColliders = CollectMeshColliders(tankNode.assembledTank.AssemblyRoot);
            tankCollidersComponent.TargetingColliders = CollectTargetColliders(tankNode.assembledTank.AssemblyRoot);
            tankCollidersComponent.TankToStaticColliders = CollectTankToStaticColliders(tankNode.assembledTank.AssemblyRoot);
            tankCollidersComponent.Extends = Quaternion.Inverse(tankCollidersComponent.TankToTankCollider.transform.rotation) * tankCollidersComponent.TankToTankCollider.bounds.extents;
            tankCollidersComponent.TankToStaticTopCollider = tankCollidersUnity.GetTankToStaticTopCollider();
            SetCollidersLayer(tankCollidersComponent, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
            SetGameObjectLayers(tankCollidersComponent.TankToStaticColliders, Layers.TANK_TO_STATIC);
            tankNode.Entity.AddComponent(tankCollidersComponent);
        }

        [OnEventFire]
        public void SetCollidersForActiveTank(NodeAddedEvent evt, ActiveTankNode tankNode) {
            SetCollidersLayer(tankNode.tankColliders, Layers.TANK_TO_TANK, Layers.TARGET, Layers.TANK_PART_VISUAL);
            tankNode.tankColliders.TankToTankCollider.enabled = true;
            tankNode.tankColliders.BoundsCollider.enabled = true;
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
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteActiveTankCollidersNode node) {
            node.tankColliders.BoundsCollider.enabled = true;
        }

        [OnEventComplete]
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteDeadTankCollidersNode node) {
            node.tankColliders.BoundsCollider.enabled = true;
        }

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeAddedEvent evt, SelfSemiActiveTankNode node) {
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_SEMIACTIVE_TANK_BOUNDS;
        }

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeRemoveEvent evt, SingleNode<TankSemiActiveStateComponent> semiActive, [JoinSelf] SelfTankNode node) {
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_TANK_BOUNDS;
        }

        [OnEventFire]
        public void SetCollidersForDeadTank(NodeAddedEvent evt, DeadTankNode tankNode) {
            SetCollidersLayer(tankNode.tankColliders, Layers.TANK_TO_TANK, Layers.DEAD_TARGET, Layers.TANK_PART_VISUAL);
            tankNode.tankColliders.TankToTankCollider.enabled = false;
        }

        [OnEventFire]
        public void SetCollidersForSpawnTank(NodeAddedEvent evt, TankSpawnStateNode tankNode) {
            SetCollidersLayer(tankNode.tankColliders, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
            tankNode.tankColliders.TankToTankCollider.enabled = false;
        }

        [OnEventFire]
        public void SetCollidersForNewTank(NodeAddedEvent evt, TankNewStateNode tankNode) {
            if (!(tankNode.assembledTank.AssemblyRoot == null)) {
                SetCollidersLayer(tankNode.tankColliders, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
                tankNode.tankColliders.TankToTankCollider.enabled = false;
            }
        }

        List<GameObject> CollectTargetColliders(GameObject root) => (from c in root.GetComponentsInChildren<Collider>(true)
                                                                     select c.gameObject
                                                                     into go
                                                                     where go.name == TankCollidersUnityComponent.TARGETING_COLLIDER_NAME
                                                                     select go).ToList();

        List<GameObject> CollectMeshColliders(GameObject root) => (from c in root.GetComponentsInChildren<VisualTriggerMarkerComponent>(true)
                                                                   select c.gameObject).ToList();

        List<Collider> CollectTankToStaticColliders(GameObject root) => (from go in root.GetComponentsInChildren<Collider>(true)
                                                                         where go.transform.parent.gameObject.name == TankCollidersUnityComponent.TANK_TO_STATIC_COLLIDER_NAME
                                                                         select go).ToList();

        void SetCollidersLayer(TankCollidersComponent tankColliders, int tankToTankLayer, int targetingLayer, int meshColliderLayer) {
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

            public HullInstanceComponent hullInstance;

            public RigidbodyComponent rigidbody;

            public TankCollidersUnityComponent tankCollidersUnity;

            public TankGroupComponent tankGroup;
        }

        public class TankCollidersNode : TankNode {
            public TankCollidersComponent tankColliders;
        }

        public class ActiveTankNode : TankCollidersNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class DeadTankNode : TankCollidersNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfTankNode : TankCollidersNode {
            public SelfTankComponent selfTank;
        }

        public class SelfSemiActiveTankNode : SelfTankNode {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class RemoteTankCollidersNode : TankCollidersNode {
            public RemoteTankComponent remoteTank;
        }

        public class RemoteActiveTankCollidersNode : RemoteTankCollidersNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class RemoteDeadTankCollidersNode : RemoteTankCollidersNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class DeadTransparentTankNode : TankCollidersNode {
            public TankDeadStateComponent tankDeadState;

            public TransparencyTransitionComponent transparencyTransition;
        }

        public class TankSpawnStateNode : TankCollidersNode {
            public TankSpawnStateComponent tankSpawnState;
        }

        public class TankNewStateNode : TankCollidersNode {
            public TankNewStateComponent tankNewState;
        }
    }
}