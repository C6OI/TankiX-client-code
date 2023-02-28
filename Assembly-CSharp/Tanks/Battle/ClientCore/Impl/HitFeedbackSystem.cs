using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class HitFeedbackSystem : ECSSystem {
        [OnEventFire]
        public void AddStreamWeaponHitFeedback(SelfHitEvent e, StreamWeaponWorkingFeedbackControllerNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle) {
            if (ValidateSelfHit(e, tank, battle)) {
                weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
            } else {
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
            }
        }

        [OnEventFire]
        public void RemoveStreamWeaponHitFeedback(SelfHitSkipEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon,
            [JoinSelf] StreamWeaponWorkingFeedbackControllerNode streamWeapon) {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void RemoveStreamWeaponHitFeedback(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [Context] [JoinSelf] StreamWeaponIdleFeedbackControllerNode idle) {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void FinishStreamWeaponHitFeedback(NodeRemoveEvent e, StreamHitNode node, [JoinSelf] SingleNode<StreamHitEnemyFeedbackComponent> weapon) {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void FinishStreamWeaponHitFeedback(NodeRemoveEvent e, StreamHitNode node, [JoinSelf] SingleNode<StreamHitTeammateFeedbackComponent> weapon) {
            weapon.Entity.RemoveComponent<StreamHitTeammateFeedbackComponent>();
        }

        [OnEventFire]
        public void BeginStreamWeaponHitFeedback(NodeAddedEvent e, StreamHitNode weapon, [Context] [JoinByTank] SelfTankNode tank, [Context] [JoinByBattle] DMBattleNode battle) {
            weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void BeginStreamWeaponHitFeedback(NodeAddedEvent e, StreamHitNode weapon, [Context] [JoinByTank] SelfTankTeamNode tank, [Context] [JoinByBattle] TeamBattleNode battle) {
            UpdateStreamWeaponHitFeedback(weapon, tank);
        }

        void UpdateStreamWeaponHitFeedback(StreamHitNode weapon, SelfTankTeamNode tank) {
            if (weapon.streamHit.TankHit == null) {
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitTeammateFeedbackComponent>();
                return;
            }

            long key = tank.teamGroup.Key;
            long key2 = weapon.streamHit.TankHit.Entity.GetComponent<TeamGroupComponent>().Key;

            if (key == key2) {
                weapon.Entity.AddComponentIfAbsent<StreamHitTeammateFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
            } else {
                weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitTeammateFeedbackComponent>();
            }
        }

        [OnEventFire]
        public void ScheduleHitFeedbackOnSelfSplashHitEvent(SelfSplashHitEvent e, SplashWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle) {
            if (ValidateSelfHit(e, tank, battle)) {
                ScheduleEvent<HitFeedbackEvent>(tank);
            } else if (e.SplashTargets != null && e.SplashTargets.Count != 0 && ValidateTargets(e.SplashTargets, tank, battle)) {
                ScheduleEvent<HitFeedbackEvent>(tank);
            }
        }

        [OnEventFire]
        public void ScheduleHitFeedbackOnSelfHitEvent(SelfHitEvent e, DiscreteWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle) {
            if (ValidateSelfHit(e, tank, battle)) {
                ScheduleEvent<HitFeedbackEvent>(tank);
            }
        }

        bool ValidateSelfHit(SelfHitEvent e, SelfTankNode tank, BattleNode battle) {
            if (e.Targets == null) {
                return false;
            }

            if (e.Targets.Count == 0) {
                return false;
            }

            return ValidateTargets(e.Targets, tank, battle);
        }

        bool ValidateTargets(List<HitTarget> targets, SelfTankNode tank, BattleNode battle) {
            for (int i = 0; i < targets.Count; i++) {
                HitTarget hitTarget = targets[i];

                if (ValidateTarget(hitTarget.Entity, tank, battle)) {
                    return true;
                }
            }

            return false;
        }

        bool ValidateTarget(Entity targetEntity, SelfTankNode tank, BattleNode battle) {
            if (targetEntity.Equals(tank.Entity)) {
                return false;
            }

            if (!targetEntity.HasComponent<TankActiveStateComponent>()) {
                return false;
            }

            if (battle.Entity.HasComponent<TeamBattleComponent>() && targetEntity.GetComponent<TeamGroupComponent>().Key == tank.Entity.GetComponent<TeamGroupComponent>().Key) {
                return false;
            }

            return true;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
        }

        public class DMBattleNode : BattleNode {
            public DMComponent dm;
        }

        public class TeamBattleNode : BattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class SelfTankNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankTeamNode : SelfTankNode {
            public TeamGroupComponent teamGroup;
        }

        public class SplashWeaponNode : WeaponNode {
            public DiscreteWeaponComponent discreteWeapon;

            public SplashWeaponComponent splashWeapon;
        }

        [Not(typeof(SplashWeaponComponent))]
        public class DiscreteWeaponNode : WeaponNode {
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class StreamHitNode : WeaponNode {
            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class StreamWeaponFeedbackControllerNode : WeaponNode {
            public StreamWeaponComponent streamWeapon;

            public StreamWeaponSimpleFeedbackControllerComponent streamWeaponSimpleFeedbackController;
        }

        public class StreamWeaponWorkingFeedbackControllerNode : StreamWeaponFeedbackControllerNode {
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class StreamWeaponIdleFeedbackControllerNode : StreamWeaponFeedbackControllerNode {
            public StreamWeaponIdleComponent streamWeaponIdle;
        }
    }
}