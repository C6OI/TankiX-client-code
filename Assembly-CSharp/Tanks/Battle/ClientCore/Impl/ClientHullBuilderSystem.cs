using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientHullBuilderSystem : ECSSystem {
        [OnEventFire]
        public void StartPrepareHull(NodeAddedEvent e, TankNode tank, [Context] [JoinByTank] HullSkin hullSkin) {
            Entity entity = tank.Entity;
            entity.AddComponent<ChassisConfigComponent>();
            NewEvent<RequestHullPrefabsEvent>().Attach(entity).Attach(hullSkin.Entity).ScheduleDelayed(0.2f);
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
        public void RequestHullInstantiating(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [Combine] PrefabLoadedNode node, [JoinByTank] HullSkin hullSkin) {
            NewEvent<InstantiateHullEvent>().Attach(hullSkin).Attach(node).ScheduleDelayed(0.2f);
        }

        [OnEventFire]
        public void InstantiateHull(InstantiateHullEvent e, HullSkin hullSkin, [JoinByTank] TankNode tank, PrefabLoadedNode node) {
            GameObject original = (GameObject)hullSkin.resourceData.Data;
            Entity entity = tank.Entity;
            GameObject gameObject = Object.Instantiate(original);
            gameObject.SetActive(false);

            if (!entity.HasComponent<HullInstanceComponent>()) {
                HullInstanceComponent hullInstanceComponent = new();
                hullInstanceComponent.HullInstance = gameObject;
                HullInstanceComponent component = hullInstanceComponent;
                entity.AddComponent(component);
            } else {
                entity.GetComponent<HullInstanceComponent>().HullInstance = gameObject;
            }

            Rigidbody rigidbody = BuildRigidBody(gameObject);
            entity.AddComponent(new RigidbodyComponent(rigidbody));
            PhysicsUtil.SetGameObjectLayer(gameObject, Layers.INVISIBLE_PHYSICS);
            gameObject.AddComponent<NanFixer>().Init(rigidbody, gameObject.transform, tank.Entity.GetComponent<UserGroupComponent>().Key);
            NewEvent<InstantiateTankCommonPartEvent>().Attach(node).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void InstantiateTankCommonPart(InstantiateTankCommonPartEvent e, [Combine] PrefabLoadedNode node) {
            HullInstanceIsReadyEvent hullInstanceIsReadyEvent = new();
            hullInstanceIsReadyEvent.HullInstance = Object.Instantiate(node.resourceData.Data as GameObject);
            HullInstanceIsReadyEvent eventInstance = hullInstanceIsReadyEvent;
            NewEvent(eventInstance).Attach(node).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void BuildHull(HullInstanceIsReadyEvent evt, PrefabLoadedNode node) {
            node.Entity.AddComponent(new TankCommonInstanceComponent(evt.HullInstance));
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

        public class HullSkin : Node {
            public HullSkinBattleItemComponent hullSkinBattleItem;

            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
        }

        public class HullInstanceIsReadyEvent : Event {
            public GameObject HullInstance;
        }
    }
}