using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class IndicationFllagRemotenessSystem : ECSSystem {
        [OnEventFire]
        public void StartFlashIconFlag(NodeAddedEvent e, [Combine] FlagCarrierNode flag, [JoinByTeam] TeamNode flagTeam,
            [JoinAll] SelfTankNode selfTank, [JoinByTeam] TeamNode selfTeam, SingleNode<FlagIconComponent> flagIcon) {
            if (flagTeam.Entity.Equals(selfTeam.Entity)) {
                FlagIconComponent component = flagIcon.component;
                component.Animator.SetBool(component.FlashStateName, true);
            }
        }

        [OnEventFire]
        public void UpdateFlashIconFlag(UpdateEvent e, FlagCarrierNode flag, [JoinByTank] TankNode carrierTank,
            [JoinByTeam] TeamNode flagTeam, [JoinByTeam] BaseNode flagBase, [JoinAll] SelfTankNode selfTank,
            [JoinByTeam] TeamNode team, [JoinByTeam] BaseNode selfBase, [JoinAll] SingleNode<FlagIconComponent> flagIcon) {
            if (!flagTeam.Entity.Equals(team.Entity)) {
                Vector3 position = carrierTank.hullInstance.HullInstance.transform.position;
                Vector3 position2 = selfBase.flagPedestal.Position;
                Vector3 position3 = flagBase.flagPedestal.Position;
                Vector3 vector = position3 - position2;
                Vector3 normalized = vector.normalized;
                float b = Vector3.Dot(normalized, position3 - position);
                b = Mathf.Max(0f, b);
                float magnitude = vector.magnitude;
                b = Mathf.Min(magnitude, b);
                float num = b / magnitude;
                FlagIconComponent component = flagIcon.component;
                float value = component.MinSpeedFlash + (component.MaxSpeedFlash - component.MinSpeedFlash) * num;
                component.Animator.SetFloat(component.SpeedFactorName, value);
            }
        }

        [OnEventFire]
        public void ExplosionIconFlag(FlagDeliveryEvent e, SingleNode<FlagComponent> flag, [JoinByTeam] TeamNode flagTeam,
            [JoinAll] SelfTankNode selfTank, [JoinByTeam] TeamNode selfTeam,
            [JoinAll] SingleNode<FlagIconComponent> flagIcon) {
            if (flagTeam.Entity.Equals(selfTeam.Entity)) {
                FlagIconComponent component = flagIcon.component;
                StopFlash(component);
                component.Animator.SetTrigger(component.ExplosionTrigerName);
            }
        }

        [OnEventFire]
        public void StopFlashIconFlag(FlagReturnEvent e, SingleNode<FlagComponent> flag, [JoinByTeam] TeamNode flagTeam,
            [JoinAll] SelfTankNode selfTank, [JoinByTeam] TeamNode selfTeam,
            [JoinAll] SingleNode<FlagIconComponent> flagIcon) {
            if (flagTeam.Entity.Equals(selfTeam.Entity)) {
                StopFlash(flagIcon.component);
            }
        }

        [OnEventFire]
        public void StopFlashIconFlag(NodeRemoveEvent e, SingleNode<RoundActiveStateComponent> round,
            [Combine] [JoinAll] SingleNode<FlagIconComponent> flagIcon) => StopFlash(flagIcon.component);

        void StopFlash(FlagIconComponent icon) => icon.Animator.SetBool(icon.FlashStateName, false);

        public class FlagCarrierNode : Node {
            public FlagComponent flag;

            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public HullInstanceComponent hullInstance;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
        }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
        }

        public class BaseNode : Node {
            public FlagPedestalComponent flagPedestal;
        }
    }
}