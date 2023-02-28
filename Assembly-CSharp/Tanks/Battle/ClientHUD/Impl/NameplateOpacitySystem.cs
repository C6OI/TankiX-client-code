using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateOpacitySystem : ECSSystem {
        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate, [Context] [JoinByTank] TankInvisibilityEffectIdleStateNode tank) {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate, [Context] [JoinByTank] TankInvisibilityEffectDeactivationStateNode tank) {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, DMBattleNode dm, [Context] [JoinByBattle] SelfBattleUserNode selfBattleUser, [Combine] NameplateNode nameplate,
            [Context] [JoinByTank] [Combine] TankInvisibilityEffectWorkingStateNode tank) {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, DMBattleNode dm, [Context] [JoinByBattle] SelfBattleUserNode selfBattleUser, [Combine] NameplateNode nameplate,
            [Context] [JoinByTank] [Combine] TankInvisibilityEffectActivationStateNode tank) {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, TeamBattleNode teamBattle, [Context] [JoinByBattle] SelfBattleUserInTeamModeNode selfBattleUser,
            [Context] [JoinByTeam] TeamNode selfTeam, [Combine] NameplateNode nameplate, [Context] [JoinByTank] [Combine] TankInTeamInvisibilityEffectWorkingStateNode tank,
            [Context] [JoinByTeam] [Combine] TeamNode tankTeam) {
            if (!selfTeam.Entity.Equals(tankTeam.Entity)) {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
            }
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, TeamBattleNode teamBattle, [Context] [JoinByBattle] SelfBattleUserInTeamModeNode selfBattleUser,
            [Context] [JoinByTeam] TeamNode selfTeam, [Combine] NameplateNode nameplate, [Context] [JoinByTank] [Combine] TankInTeamInvisibilityEffectActivationStateNode tank,
            [Context] [JoinByTeam] [Combine] TeamNode tankTeam) {
            if (!selfTeam.Entity.Equals(tankTeam.Entity)) {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
            }
        }

        [OnEventFire]
        public void ToConcealmentState(TimeUpdateEvent e, NameplateAppearanceNode nameplate) {
            if (!nameplate.nameplate.alwaysVisible && nameplate.nameplatePosition.sqrDistance > nameplate.nameplateOpacity.sqrConcealmentDistance) {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateConcealmentState>();
            }
        }

        [OnEventFire]
        public void ToAppearanceState(TimeUpdateEvent e, NameplateConclealmentNotInvisibilityNode nameplate) {
            SwitchToAppearanceByDistance(nameplate);
        }

        void SwitchToAppearanceByDistance(NameplateNode nameplate) {
            if (nameplate.nameplatePosition.sqrDistance < nameplate.nameplateOpacity.sqrConcealmentDistance) {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
            }
        }

        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate) {
            nameplate.nameplate.Alpha = 0f;
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void RevealNameplate(TimeUpdateEvent e, NameplateAppearanceNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            IncreaseAlpha(nameplate2, e.DeltaTime);
        }

        [OnEventFire]
        public void HideNameplate(TimeUpdateEvent e, NameplateConclealmentNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;

            if (!nameplate.nameplate.alwaysVisible && nameplate2.Alpha > 0f) {
                DecreaseAlpha(nameplate2, e.DeltaTime);
            }
        }

        void IncreaseAlpha(NameplateComponent nameplateComponent, float dt) {
            float deltaAlpha = nameplateComponent.appearanceSpeed * dt;

            if (nameplateComponent.Alpha < 1f) {
                ChangeAlpha(nameplateComponent, deltaAlpha);
            }
        }

        void DecreaseAlpha(NameplateComponent nameplateComponent, float dt) {
            float deltaAlpha = (0f - nameplateComponent.disappearanceSpeed) * dt;
            ChangeAlpha(nameplateComponent, deltaAlpha);
        }

        [OnEventFire]
        public void StopOpacityChange(NodeAddedEvent e, NameplateDeletionNode nameplate) {
            nameplate.Entity.RemoveComponent<NameplateOpacityComponent>();
        }

        [OnEventFire]
        public void DeleteNameplateOnReincarnation(NodeRemoveEvent e, TankIncarnationNode tank, [JoinByTank] SingleNode<NameplateComponent> nameplate) {
            NameplateComponent component = nameplate.component;
            Object.Destroy(component.gameObject);
        }

        [OnEventFire]
        public void DeleteNameplate(TimeUpdateEvent e, NameplateDeletionNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            DecreaseAlpha(nameplate2, e.DeltaTime);

            if (nameplate2.Alpha <= 0f) {
                Object.Destroy(nameplate2.gameObject);
            }
        }

        void ChangeAlpha(NameplateComponent nameplate, float deltaAlpha) {
            nameplate.Alpha = Mathf.Clamp01(nameplate.Alpha + deltaAlpha);
        }

        public class NameplateNode : Node {
            public NameplateComponent nameplate;

            public NameplateESMComponent nameplateEsm;

            public NameplateOpacityComponent nameplateOpacity;

            public NameplatePositionComponent nameplatePosition;

            public TankGroupComponent tankGroup;
        }

        public class NameplateAppearanceNode : NameplateNode {
            public NameplateAppearanceStateComponent nameplateAppearanceState;
        }

        public class NameplateConclealmentNode : NameplateNode {
            public NameplateConcealmentStateComponent nameplateConcealmentState;
        }

        [Not(typeof(NameplateInvisibilityEffectStateComponent))]
        public class NameplateConclealmentNotInvisibilityNode : NameplateConclealmentNode { }

        public class NameplateDeletionNode : Node {
            public NameplateComponent nameplate;
            public NameplateDeletionStateComponent nameplateDeletionState;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }

        public class TankNode : Node {
            public RemoteTankComponent remoteTank;

            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class TankInvisibilityEffectIdleStateNode : TankNode {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class TankInvisibilityEffectActivationStateNode : TankNode {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class TankInTeamInvisibilityEffectActivationStateNode : TankInvisibilityEffectActivationStateNode {
            public TeamGroupComponent teamGroup;
        }

        public class TankInvisibilityEffectDeactivationStateNode : TankNode {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class TankInvisibilityEffectWorkingStateNode : TankNode {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class TankInTeamInvisibilityEffectWorkingStateNode : TankInvisibilityEffectWorkingStateNode {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfBattleUserInTeamModeNode : SelfBattleUserNode {
            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node {
            public TeamComponent team;

            public TeamGroupComponent teamGroup;
        }

        public class DMBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public DMComponent dm;
        }

        public class TeamBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public TeamBattleComponent teamBattle;
        }
    }
}