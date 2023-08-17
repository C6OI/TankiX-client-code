using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientHullBuilderSystem : ECSSystem {
        [OnEventFire]
        public void StartPrepareHull(NodeAddedEvent e, TankNode tank, [JoinByTank] [Context] HullSkin hullSkin) {
            Entity entity = tank.Entity;
            entity.AddComponent<ChassisConfigComponent>();
            NewEvent<RequestHullPrefabsEvent>().Attach(entity).Attach(hullSkin.Entity).ScheduleDelayed(0.1f);
        }

        [OnEventFire]
        public void RequestPrefabs(RequestHullPrefabsEvent e, TankNode tank, HullSkin hullSkin) {
            Entity entity = tank.Entity;
            entity.AddComponent<TankCommonPrefabComponent>();
            TankCommonPrefabComponent component = entity.GetComponent<TankCommonPrefabComponent>();
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(component.AssetGuid)));
            entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestHullInstantiating(NodeAddedEvent e, SingleNode<MapInstanceComponent> map,
            [Combine] PrefabLoadedNode node, [JoinByTank] HullSkin hullSkin) {
            NewEvent<InstantiateHullEvent>().Attach(hullSkin).ScheduleDelayed(0.1f);
            NewEvent<InstantiateTankCommonPartEvent>().Attach(node).ScheduleDelayed(0.2f);
        }

        [OnEventFire]
        public void InstantiateHull(InstantiateHullEvent e, HullSkin hullSkin, [JoinByTank] TankNode tank) {
            GameObject original = (GameObject)hullSkin.resourceData.Data;
            Entity entity = tank.Entity;
            HullInstanceComponent hullInstanceComponent = new();
            hullInstanceComponent.HullInstance = Object.Instantiate(original);
            hullInstanceComponent.HullInstance.SetActive(false);
            entity.AddComponent(hullInstanceComponent);
            Rigidbody rigidbody = BuildRigidBody(hullInstanceComponent.HullInstance);
            entity.AddComponent(new RigidbodyComponent(rigidbody));
            PhysicsUtil.SetGameObjectLayer(hullInstanceComponent.HullInstance, Layers.INVISIBLE_PHYSICS);
        }

        [OnEventFire]
        public void InstantiateTankCommonPart(InstantiateTankCommonPartEvent e, [Combine] PrefabLoadedNode node) {
            GameObject original = (GameObject)node.resourceData.Data;
            node.Entity.AddComponent(new TankCommonInstanceComponent(Object.Instantiate(original)));
        }

        Rigidbody BuildRigidBody(GameObject hullInstance) {
            Rigidbody rigidbody = hullInstance.AddComponent<Rigidbody>();
            rigidbody.mass = 1100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0.05f;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.sleepThreshold = 0.1f;
            return rigidbody;
        }

        public class TankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class PrefabLoadedNode : Node {
            public ResourceDataComponent resourceData;
            public TankCommonPrefabComponent tankCommonPrefab;

            public TankGroupComponent tankGroup;
        }

        public class ReadySelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class HullSkin : Node {
            public HullSkinBattleItemComponent hullSkinBattleItem;

            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
        }
    }
}