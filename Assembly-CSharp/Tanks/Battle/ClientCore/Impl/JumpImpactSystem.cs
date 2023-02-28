using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class JumpImpactSystem : ECSSystem {
        const float HUNTER_BODY_MASS = 3000f;

        const float NORMAL_GRAVITY_FORCE = 9.8f;

        [OnEventFire]
        public void EffectAdded(NodeAddedEvent e, JumpEffectNode node, [JoinByTank] TankNode tank) {
            GameObject jumpImpactEffect = tank.moduleVisualEffectObjects.JumpImpactEffect;

            if (!jumpImpactEffect.activeInHierarchy) {
                jumpImpactEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                jumpImpactEffect.SetActive(true);
            }

            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            float num = node.jumpEffect.BaseImpact * node.jumpEffectConfig.ForceUpgradeMult;
            float num2 = !node.jumpEffect.ScaleByMass ? 1f : rigidbody.mass / 3000f;
            float num3 = node.jumpEffect.GravityPenalty * (1f - Physics.gravity.magnitude / 9.8f) * num;
            Vector3 vector = !node.jumpEffect.AlwaysUp ? rigidbody.rotation * Vector3.up : Vector3.up;
            Vector3 force = num2 * (num - num3) * vector;
            ImpactEvent impactEvent = new();
            impactEvent.LocalHitPoint = rigidbody.centerOfMass;
            impactEvent.Force = force;
            ImpactEvent eventInstance = impactEvent;
            ScheduleEvent(eventInstance, tank);
        }

        public class JumpEffectNode : Node {
            public JumpEffectComponent jumpEffect;

            public JumpEffectConfigComponent jumpEffectConfig;
        }

        public class TankNode : Node {
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;

            public RigidbodyComponent rigidbody;
            public TankActiveStateComponent tankActiveState;
        }
    }
}