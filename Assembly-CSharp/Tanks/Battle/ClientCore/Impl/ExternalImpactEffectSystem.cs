using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ExternalImpactEffectSystem : ECSSystem {
        readonly float SPLASH_CENTER_HEIGHT = 0.4f;

        [OnEventFire]
        public void CollectSplashTarget(StartSplashEffectEvent e, SplashEffectNode effect, [JoinByTank] SelfTankNode selfTank, [JoinByTeam] ICollection<RemoteTankNode> remoteTanks) {
            StaticHit staticHit = new();
            staticHit.Normal = Vector3.up;
            staticHit.Position = selfTank.rigidbody.Rigidbody.position + SPLASH_CENTER_HEIGHT * (selfTank.rigidbody.Rigidbody.rotation * Vector3.up);
            StaticHit staticHit2 = staticHit;
            SplashHitData splashHitData = SplashHitData.CreateSplashHitData(new List<HitTarget>(), staticHit2, effect.Entity);
            splashHitData.ExcludedEntityForSplashHit = new HashSet<Entity> { selfTank.Entity };

            if (!effect.splashEffect.CanTargetTeammates) {
                foreach (RemoteTankNode remoteTank in remoteTanks) {
                    splashHitData.ExcludedEntityForSplashHit.Add(remoteTank.Entity);
                }
            }

            ScheduleEvent<SendTankMovementEvent>(selfTank);
            ScheduleEvent(new CollectSplashTargetsEvent(splashHitData), effect);
        }

        [OnEventFire]
        public void EnableEffect(NodeAddedEvent e, ExternalImpactEffectNode effectNode, [JoinByTank] TankNode tank, [JoinAll] SelfTankNode selfTank) {
            GameObject externalImpactEffect = tank.moduleVisualEffectObjects.ExternalImpactEffect;

            if (!externalImpactEffect.activeInHierarchy) {
                externalImpactEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                externalImpactEffect.SetActive(true);
            }

            ScheduleEvent<StartSplashEffectEvent>(effectNode);

            ScheduleEvent(new TankFallEvent {
                FallingPower = 100f,
                FallingType = TankFallingType.NOTHING
            }, selfTank);
        }

        public class EffectNode : Node {
            public EffectComponent effect;
        }

        public class SplashEffectNode : EffectNode {
            public SplashEffectComponent splashEffect;

            public SplashWeaponComponent splashWeapon;
        }

        public class ExternalImpactEffectNode : SplashEffectNode {
            public ExternalImpactEffectComponent externalImpactEffect;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public BattleGroupComponent battleGroup;

            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;

            public RigidbodyComponent rigidbody;

            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }
    }
}