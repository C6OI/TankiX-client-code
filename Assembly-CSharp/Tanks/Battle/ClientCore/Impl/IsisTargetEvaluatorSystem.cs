using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IsisTargetEvaluatorSystem : ECSSystem {
        [OnEventFire]
        public void AddComponent(NodeAddedEvent e, WeaponNode weapon) {
            weapon.Entity.AddComponent<IsisTargetEvaluatorComponent>();
        }

        [OnEventComplete]
        public void SaveLastDirection(TargetingEvent e, EvaluatorNode weapon) {
            if (e.TargetingData.HasTargetHit()) {
                Log.Debug("SaveLastDirection");
                weapon.isisTargetEvaluator.LastDirectionIndex = e.TargetingData.BestDirectionIndex;
            }
        }

        [OnEventFire]
        public void ClearLastDirection(NodeRemoveEvent e, SingleNode<StreamHitComponent> weaponContext, [JoinSelf] SingleNode<IsisTargetEvaluatorComponent> weaponJoin) {
            Log.Debug("ClearLastDirection");
            weaponJoin.component.LastDirectionIndex = null;
        }

        [OnEventFire]
        public void EvaluateLastDirection(TargetingEvaluateEvent e, EvaluatorNode weapon) {
            int? lastDirectionIndex = weapon.isisTargetEvaluator.LastDirectionIndex;

            if (lastDirectionIndex.HasValue) {
                Log.Debug("EvaluateLastDirection");
                e.TargetingData.Directions[lastDirectionIndex.Value].Priority += 10f;
            }
        }

        [OnEventFire]
        public void EvaluateLastTank(TargetingEvaluateEvent e, HitNode weapon) {
            List<DirectionData>.Enumerator enumerator = e.TargetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                List<TargetData>.Enumerator enumerator2 = enumerator.Current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current = enumerator2.Current;

                    if (current.TargetEntity == weapon.streamHit.TankHit.Entity) {
                        Log.Debug("EvaluateLastTank");
                        current.Priority += 100f;
                    }
                }
            }
        }

        [OnEventFire]
        public void ResendEvaluateTeamTank(TargetingEvaluateEvent e, WeaponNode weapon, [JoinByTank] TeamTankNode shooterTank) {
            TargetingData targetingData = e.TargetingData;
            long key = shooterTank.teamGroup.Key;
            List<DirectionData>.Enumerator enumerator = targetingData.Directions.GetEnumerator();

            while (enumerator.MoveNext()) {
                DirectionData current = enumerator.Current;
                List<TargetData>.Enumerator enumerator2 = current.Targets.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TargetData current2 = enumerator2.Current;
                    NewEvent(new IsisEvaluateTeamTankEvent(current2, key)).Attach(weapon).Attach(current2.TargetEntity).Schedule();
                }
            }
        }

        [OnEventFire]
        public void EvaluateTeamTank(IsisEvaluateTeamTankEvent e, WeaponNode weapon, TeamTankNode targetTank) {
            TargetData targetData = e.TargetData;
            long shooterTeamKey = e.ShooterTeamKey;
            long key = targetTank.teamGroup.Key;

            if (shooterTeamKey == key) {
                Log.Debug("EvaluateTeamTank: same team");
                targetData.Priority += 1f;
                HealthComponent health = targetTank.health;

                if (health.CurrentHealth != health.MaxHealth) {
                    Log.Debug("EvaluateTeamTank: not full health");
                    targetData.Priority += 2f;
                }

                TemperatureComponent temperature = targetTank.temperature;

                if (temperature.Temperature > 0f) {
                    Log.Debug("EvaluateTeamTank: positive temperature");
                    targetData.Priority += 5f;
                } else if (temperature.Temperature < 0f) {
                    Log.Debug("EvaluateTeamTank: negative temperature");
                    targetData.Priority += 4f;
                }
            } else {
                Log.Debug("EvaluateTeamTank: enemy team");
                targetData.Priority += 2f;
            }
        }

        public class WeaponNode : Node {
            public IsisComponent isis;
        }

        public class EvaluatorNode : Node {
            public IsisTargetEvaluatorComponent isisTargetEvaluator;
        }

        public class HitNode : Node {
            public IsisComponent isis;

            public IsisTargetEvaluatorComponent isisTargetEvaluator;

            public StreamHitComponent streamHit;
        }

        public class TeamTankNode : Node {
            public HealthComponent health;
            public TankComponent tank;

            public TeamGroupComponent teamGroup;

            public TemperatureComponent temperature;
        }
    }
}