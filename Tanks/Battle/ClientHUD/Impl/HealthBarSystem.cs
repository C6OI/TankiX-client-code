using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HealthBarSystem : ECSSystem {
        static readonly string PARTIAL_HEALTH_PREREQUISITE = "PARTIAL_HEALTH_PREREQUISITE";

        static readonly string SEMIACTIVE_PREREQUISITE = "RESPAWN_PREREQUISITE";

        [OnEventFire]
        public void AttachSelfTankHeathBar(NodeAddedEvent e, SelfTankNode tank,
            SelfNonAttachedHealthBarNode selfNonAttachedHealthBar) => tank.tankGroup.Attach(selfNonAttachedHealthBar.Entity);

        [OnEventFire]
        public void DetachSelfTankHeathBar(NodeRemoveEvent e, SelfTankNode tank,
            [JoinByTank] AttachedSelfHealthBarNode healthBar) => tank.tankGroup.Detach(healthBar.Entity);

        [OnEventFire]
        public void HideSelfHealthBarOnExitActiveState(NodeRemoveEvent e, AttachedSelfHealthBarNode selfAttachedHealthBar) =>
            selfAttachedHealthBar.visibilityPrerequisites.RemoveShowPrerequisite(PARTIAL_HEALTH_PREREQUISITE);

        [OnEventFire]
        public void ShowHealthBarOnSemiActiveState(NodeAddedEvent e, SelfSemiActiveTankNode tank,
            [JoinByTank] AttachedSelfHealthBarNode healthBar) {
            healthBar.visibilityPrerequisites.AddShowPrerequisite(SEMIACTIVE_PREREQUISITE);
            healthBar.healthBar.ActivateAnimation(tank.tankSemiActiveState.ActivationTime);
        }

        [OnEventFire]
        public void ShowHealthBarOnSemiActiveState(NodeRemoveEvent e, SelfSemiActiveTankNode tank,
            [JoinByTank] AttachedSelfHealthBarNode healthBar) =>
            healthBar.visibilityPrerequisites.RemoveShowPrerequisite(SEMIACTIVE_PREREQUISITE);

        [OnEventFire]
        public void InitHealthBarProgressOnRemoteTanks(NodeAddedEvent e, RemoteTankNode tank,
            [JoinByTank] [Context] AttachedHealthBarNode healthBar) =>
            UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);

        [OnEventFire]
        public void InitHealthBarProgressOnSelfTank(NodeAddedEvent e, SelfTankNode tank,
            [Context] [JoinByTank] AttachedSelfHealthBarNode healthBar) {
            UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);
            UpdateVisibility(healthBar.visibilityPrerequisites, healthBar.healthBar);
        }

        [OnEventFire]
        public void ChangeProgressValueOnAnyHealthBar(HealthChangedEvent e, TankNode tank,
            [JoinByTank] AttachedHealthBarNode healthBar) =>
            UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);

        [OnEventComplete]
        public void UpdateSelfHealthBarVisibility(HealthChangedEvent e, TankNode tank,
            [JoinByTank] AttachedSelfHealthBarNode healthBar) =>
            UpdateVisibility(healthBar.visibilityPrerequisites, healthBar.healthBar);

        void UpdateVisibility(VisibilityPrerequisitesComponent visibilityPrerequisites, HealthBarComponent healthBar) {
            if (healthBar.ProgressValue > 0f && healthBar.ProgressValue < 1f) {
                visibilityPrerequisites.AddShowPrerequisite(PARTIAL_HEALTH_PREREQUISITE);
            } else {
                visibilityPrerequisites.RemoveShowPrerequisite(PARTIAL_HEALTH_PREREQUISITE);
            }
        }

        void UpdateHealth(HealthComponent health, HealthConfigComponent healthConfig, HealthBarComponent healthBar) {
            float progressValue = health.CurrentHealth / healthConfig.BaseHealth;
            healthBar.ProgressValue = progressValue;
        }

        [OnEventFire]
        public void ColorizeTeamHealthBar(NodeAddedEvent e, AttachedSelfColorizedHealthBarNode healthBar,
            [Context] [JoinByTank] SelfTeamTankNode tank, [JoinByTeam] [Context] TeamColorNode teamColor) {
            switch (teamColor.teamColor.TeamColor) {
                case TeamColor.BLUE:
                    healthBar.teamUiColorize.ColorizeBlue();
                    break;

                case TeamColor.RED:
                    healthBar.teamUiColorize.ColorizeRed();
                    break;
            }
        }

        public class TankNode : Node {
            public HealthComponent health;

            public HealthConfigComponent healthConfig;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;
        }

        public class SelfTeamTankNode : SelfTankNode {
            public TeamGroupComponent teamGroup;
        }

        public class TeamColorNode : Node {
            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class SelfSemiActiveTankNode : SelfTankNode {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        [Not(typeof(TankGroupComponent))]
        public class SelfNonAttachedHealthBarNode : Node {
            public HealthBarComponent healthBar;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class AttachedSelfHealthBarNode : Node {
            public HealthBarComponent healthBar;

            public TankGroupComponent tankGroup;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class AttachedSelfColorizedHealthBarNode : AttachedSelfHealthBarNode {
            public TeamUIColorizeComponent teamUiColorize;
        }

        public class AttachedHealthBarNode : Node {
            public HealthBarComponent healthBar;

            public TankGroupComponent tankGroup;
        }
    }
}