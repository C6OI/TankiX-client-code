using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class DroneEffectSystem : ECSSystem {
        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, SingleNode<MapInstanceComponent> map, [Combine] DroneLoadedNode drone,
            [Context] [JoinByUser] TankNode tank, [JoinByTank] SingleNode<TankIncarnationComponent> incarnation, [JoinByUser] Optional<SingleNode<UserAvatarComponent>> avatar) {
            string key = !avatar.IsPresent() || !(avatar.Get().component.Id == "457e8f5f-953a-424c-bd97-67d9e116ab7a") ? "drone" : "droneHolo";
            GameObject gameObject = mapEffect.component.PreloadedEffects[key];

            if ((bool)gameObject) {
                GameObject gameObject2 = Object.Instantiate(gameObject, null);
                gameObject2.SetActive(true);
                Rigidbody component = gameObject2.GetComponent<Rigidbody>();
                RigidbodyComponent rigidbodyComponent = new();
                rigidbodyComponent.Rigidbody = component;
                drone.Entity.AddComponent(rigidbodyComponent);
                component.GetComponent<EntityBehaviour>().BuildEntity(drone.Entity);
                DroneOwnerComponent droneOwnerComponent = new();
                droneOwnerComponent.Incarnation = incarnation.Entity;
                droneOwnerComponent.Rigidbody = tank.rigidbody.Rigidbody;
                DroneOwnerComponent component2 = droneOwnerComponent;
                drone.Entity.AddComponent(component2);
                drone.Entity.AddComponent(new EffectInstanceComponent(gameObject2));
            }
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, [Combine] SelfDroneNode spider, [JoinByBattle] SingleNode<TeamBattleComponent> battle) {
            spider.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, [Combine] SelfDroneNode spider, [JoinByBattle] SingleNode<CTFComponent> battle) {
            spider.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Targeting(UpdateEvent e, SelfDroneWithoutTargetNode drone) {
            ScheduleEvent<UnitSelectTargetEvent>(drone);
        }

        [OnEventFire]
        public void Targeting(UpdateEvent e, SelfDroneWithTargetNode drone) {
            Entity target = drone.unitTarget.Target;

            if (!target.Alive || !target.HasComponent<TankActiveStateComponent>()) {
                drone.Entity.RemoveComponent<UnitTargetComponent>();
            }
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class DroneLoadedNode : Node {
            public DroneEffectComponent droneEffect;

            public UnitGroupComponent unitGroup;

            public UnitMoveComponent unitMove;
        }

        public class DroneNode : DroneLoadedNode {
            public RigidbodyComponent rigidbody;
        }

        public class SelfDroneNode : DroneNode {
            public SelfComponent self;
        }

        public class SelfDroneWithTargetNode : SelfDroneNode {
            public UnitTargetComponent unitTarget;
        }

        [Not(typeof(UnitTargetComponent))]
        public class SelfDroneWithoutTargetNode : SelfDroneNode { }
    }
}