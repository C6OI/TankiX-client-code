using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientHUD.Impl;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraShakerSystem : ECSSystem {
        [OnEventFire]
        public void ShakeOnDeath(NodeAddedEvent e, SelfTankDeadNode tank, CameraShakerOnDeathNode cameraShaker, SingleNode<GameCameraShakerSettingsComponent> settings,
            [JoinAll] SingleNode<MainHUDComponent> hud) {
            cameraShaker.cameraShaker.ShakeOnce(settings.component, cameraShaker.tankCameraShakerConfigOnDeath);
            hud.component.battleHudRoot.ShakeHUDOnDeath(settings.component, cameraShaker.tankCameraShakerConfigOnDeath);
        }

        [OnEventFire]
        public void ShakeOnShot(SelfShotEvent e, NotVulcanKickbackNode weapon, [JoinByTank] SelfTankNode tank, [JoinAll] CameraShakerNode cameraShaker,
            [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings, [JoinAll] SingleNode<MainHUDComponent> hud) {
            cameraShaker.cameraShaker.ShakeOnce(settings.component, weapon.kickbackCameraShakerConfig, weapon.weaponCooldown.CooldownIntervalSec);
            hud.component.battleHudRoot.ShakeHUDOnShot(settings.component, weapon.kickbackCameraShakerConfig);
        }

        [OnEventFire]
        public void ShakeOnVulcanShooting(NodeAddedEvent evt, VulcanShootingNode weapon, [Context] [JoinByTank] SelfTankNode tank, CameraShakerNode cameraShaker,
            SingleNode<GameCameraShakerSettingsComponent> settings) {
            CameraShakeInstance cameraShakeInstance = cameraShaker.cameraShaker.StartShake(settings.component, weapon.kickbackCameraShakerConfig);

            if (cameraShakeInstance != null) {
                weapon.Entity.AddComponent(new VulcanShootingCameraShakerInstanceComponent(cameraShakeInstance, weapon.kickbackCameraShakerConfig.FadeOutTime));
            }
        }

        [OnEventFire]
        public void RemoveVulcanShootingCameraShakerInstance(NodeAddedEvent evt, VulcanIdleNode vulcan) {
            vulcan.Entity.RemoveComponent<VulcanShootingCameraShakerInstanceComponent>();
        }

        [OnEventFire]
        public void RemoveVulcanShootingCameraShakerInstance(NodeAddedEvent evt, VulcanSlowDownNode vulcan) {
            vulcan.Entity.RemoveComponent<VulcanShootingCameraShakerInstanceComponent>();
        }

        [OnEventFire]
        public void RemoveVulcanShootingCameraShakerInstance(NodeRemoveEvent evt, SingleNode<VulcanShootingCameraShakerInstanceComponent> weapon) {
            weapon.component.Instance.deleteOnInactive = true;
            weapon.component.Instance.StartFadeOut(weapon.component.FadeOutTime);
        }

        [OnEventFire]
        public void ShakeOnDiscreteImpact(ImpactEvent evt, SingleNode<ImpactCameraShakerConfigComponent> weapon, SelfTankNode tank, [JoinAll] CameraShakerNode cameraShaker,
            [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings, [JoinAll] SingleNode<MainHUDComponent> hud) {
            cameraShaker.cameraShaker.ShakeByDiscreteImpact(settings.component, weapon.component, evt.Force.normalized, evt.WeakeningCoeff);
            hud.component.battleHudRoot.ShakeHUDOnImpact(settings.component, weapon.component);
        }

        [OnEventFire]
        public void CheckIfVulcanHitSelfTank(NodeAddedEvent e, [Combine] VulcanStreamHitNode vulcan, CameraShakerNode cameraShaker,
            SingleNode<GameCameraShakerSettingsComponent> settings) {
            if (ValidateShakeOnVulcanImpact(vulcan, cameraShaker, settings)) {
                if (!vulcan.Entity.HasComponent<VulcanImpactCameraShakerInstanceComponent>()) {
                    vulcan.Entity.AddComponent(new VulcanImpactCameraShakerInstanceComponent(vulcan.impactCameraShakerConfig.FadeOutTime));
                } else {
                    vulcan.Entity.GetComponent<VulcanImpactCameraShakerInstanceComponent>().Init(vulcan.impactCameraShakerConfig.FadeOutTime);
                }
            }
        }

        [OnEventFire]
        public void CheckIfVulcanHitSelfTank(UpdateEvent e, VulcanStreamHitNode vulcan, [JoinAll] CameraShakerNode cameraShaker,
            [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings) {
            if (ValidateShakeOnVulcanImpact(vulcan, cameraShaker, settings) && !vulcan.Entity.HasComponent<VulcanImpactCameraShakerInstanceComponent>()) {
                vulcan.Entity.AddComponent(new VulcanImpactCameraShakerInstanceComponent(vulcan.impactCameraShakerConfig.FadeOutTime));
            }
        }

        bool ValidateShakeOnVulcanImpact(VulcanStreamHitNode vulcan, CameraShakerNode cameraShaker, SingleNode<GameCameraShakerSettingsComponent> settings) {
            StreamHitComponent streamHit = vulcan.streamHit;

            if (!settings.component.Enabled) {
                return false;
            }

            if (streamHit.TankHit == null) {
                return false;
            }

            Entity entity = streamHit.TankHit.Entity;

            if (!entity.HasComponent<SelfTankComponent>()) {
                return false;
            }

            return true;
        }

        [OnEventFire]
        public void RemoveVulcanImpactCameraShakeInstance(NodeRemoveEvent e, SingleNode<StreamHitComponent> streamHitWeapon,
            [JoinSelf] SingleNode<VulcanImpactCameraShakerInstanceComponent> vulcan) {
            vulcan.Entity.RemoveComponent<VulcanImpactCameraShakerInstanceComponent>();
        }

        [OnEventFire]
        public void RemoveVulcanImpactCameraShakeInstance(NodeRemoveEvent e, SingleNode<VulcanImpactCameraShakerInstanceComponent> vulcan) {
            if (vulcan.component.CameraShakeInstance != null) {
                vulcan.component.CameraShakeInstance.deleteOnInactive = true;
                vulcan.component.CameraShakeInstance.StartFadeOut(vulcan.component.FadeOutTime);
            }
        }

        [OnEventFire]
        public void UpdateVulcanImpactCameraShakeInstance(VulcanImpactEvent e, SingleNode<VulcanImpactCameraShakerInstanceComponent> vulcan) {
            bool flag = vulcan.component.ImpactDirection != e.Force || vulcan.component.WeakeningCoeff != e.WeakeningCoeff;
            vulcan.component.ImpactDirection = e.Force;
            vulcan.component.WeakeningCoeff = e.WeakeningCoeff;
            vulcan.component.ImpactDataChanged |= flag;
        }

        [OnEventFire]
        public void UpdateVulcanImpactCameraShakeInstance(UpdateEvent e, VulcanImpactShakerNode vulcan, [JoinAll] CameraShakerNode cameraShaker,
            [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings) {
            if (vulcan.vulcanImpactCameraShakerInstance.ImpactDataChanged) {
                vulcan.vulcanImpactCameraShakerInstance.CameraShakeInstance = cameraShaker.cameraShaker.UpdateImpactShakeInstance(settings.component,
                    vulcan.vulcanImpactCameraShakerInstance.CameraShakeInstance, vulcan.impactCameraShakerConfig, vulcan.vulcanImpactCameraShakerInstance.ImpactDirection.normalized,
                    vulcan.vulcanImpactCameraShakerInstance.WeakeningCoeff);

                vulcan.vulcanImpactCameraShakerInstance.ImpactDataChanged = false;
            }
        }

        [OnEventFire]
        public void ShakeOnFalling(TankFallEvent evt, SelfTankNode tank, [JoinAll] CameraShakerFallingNode cameraShaker, [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings,
            [JoinAll] SingleNode<MainHUDComponent> hud) {
            float fallingPower = evt.FallingPower;
            float minFallingPower = cameraShaker.tankFallingCameraShakerConfig.MinFallingPower;
            float maxFallingPower = cameraShaker.tankFallingCameraShakerConfig.MaxFallingPower;
            float weakeningCoeff = Mathf.Clamp01((fallingPower - minFallingPower) / (maxFallingPower - minFallingPower));
            cameraShaker.cameraShaker.ShakeByFalling(settings.component, cameraShaker.tankFallingCameraShakerConfig, weakeningCoeff);

            if (fallingPower >= cameraShaker.tankFallingCameraShakerConfig.MinFallingPowerForHUD) {
                hud.component.battleHudRoot.ShakeHUDOnFalling(settings.component, cameraShaker.tankFallingCameraShakerConfig);
            }
        }

        public class SelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;
        }

        public class WeaponKickbackNode : Node {
            public KickbackComponent kickback;
            public KickbackCameraShakerConfigComponent kickbackCameraShakerConfig;

            public TankGroupComponent tankGroup;

            public WeaponCooldownComponent weaponCooldown;
        }

        [Not(typeof(VulcanComponent))]
        public class NotVulcanKickbackNode : WeaponKickbackNode { }

        public class VulcanShootingNode : WeaponKickbackNode {
            public VulcanComponent vulcan;

            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class VulcanIdleNode : WeaponKickbackNode {
            public VulcanIdleComponent vulcanIdle;

            public VulcanShootingCameraShakerInstanceComponent vulcanShootingCameraShakerInstance;
        }

        public class VulcanSlowDownNode : WeaponKickbackNode {
            public VulcanShootingCameraShakerInstanceComponent vulcanShootingCameraShakerInstance;
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class SelfTankDeadNode : SelfTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class CameraShakerNode : Node {
            public CameraRootTransformComponent cameraRootTransform;
            public CameraShakerComponent cameraShaker;
        }

        public class CameraShakerOnDeathNode : CameraShakerNode {
            public TankCameraShakerConfigOnDeathComponent tankCameraShakerConfigOnDeath;
        }

        public class CameraShakerFallingNode : CameraShakerNode {
            public TankFallingCameraShakerConfigComponent tankFallingCameraShakerConfig;
        }

        public class VulcanStreamHitNode : Node {
            public ImpactCameraShakerConfigComponent impactCameraShakerConfig;

            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
            public VulcanComponent vulcan;
        }

        public class VulcanImpactShakerNode : Node {
            public ImpactCameraShakerConfigComponent impactCameraShakerConfig;

            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;

            public VulcanImpactCameraShakerInstanceComponent vulcanImpactCameraShakerInstance;
        }
    }
}