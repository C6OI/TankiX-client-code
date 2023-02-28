using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class UnitSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void InitTargeting(NodeAddedEvent evt, UnitTargetingNode unit) {
            UnitTargetingComponent unitTargeting = unit.unitTargeting;
            unit.unitTargeting.UpdateEvent = NewEvent<UnitSelectTargetEvent>().Attach(unit).SchedulePeriodic(unitTargeting.Period);
            unit.Entity.AddComponent(new TargetCollectorComponent(new TargetCollector(unit.Entity), new TargetValidator(unit.Entity)));
        }

        [OnEventFire]
        public void DisableTargeting(NodeRemoveEvent evt, UnitTargetingNode unit) {
            unit.unitTargeting.UpdateEvent.Manager().Cancel();
        }

        [OnEventFire]
        public void Targeting(UnitSelectTargetEvent evt, UnitTargetingNode unit, [JoinSelf] Optional<SingleNode<UnitTargetComponent>> unitTargetNode) {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            TargetingEvent eventInstance = BattleCache.targetingEvent.GetInstance().Init(targetingData);
            ScheduleEvent(eventInstance, unit);

            if (targetingData.BestDirection == null || !targetingData.BestDirection.HasTargetHit()) {
                return;
            }

            Entity targetEntity = targetingData.BestDirection.Targets[0].TargetEntity;
            Entity targetIncorantionEntity = targetingData.BestDirection.Targets[0].TargetIncorantionEntity;

            if (!targetEntity.HasComponent<EnemyComponent>()) {
                return;
            }

            if (unitTargetNode.IsPresent()) {
                if (unitTargetNode.Get().component.Target.Equals(targetEntity)) {
                    return;
                }

                unit.Entity.RemoveComponent<UnitTargetComponent>();
            }

            UnitTargetComponent unitTargetComponent = new();
            unitTargetComponent.Target = targetEntity;
            unitTargetComponent.TargetIncarnation = targetIncorantionEntity;
            UnitTargetComponent component = unitTargetComponent;
            unit.Entity.AddComponent(component);
        }

        public class SelfUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        public class UnitNode : Node {
            public UnitComponent unit;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UnitTargetComponent))]
        public class UnitTargetingNode : Node {
            public SelfComponent self;

            public UnitReadyComponent unitReady;

            public UnitTargetingComponent unitTargeting;
        }
    }
}