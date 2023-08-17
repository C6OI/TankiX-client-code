using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamWeaponParticleSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, StreamWeaponEffectInitNode node,
            [JoinAll] SingleNode<StreamWeaponSettingsComponent> settings) {
            node.streamEffect.Init(node.muzzlePoint);
            node.streamEffect.Instance.ApplySettings(settings.component);
            node.Entity.AddComponent<StreamEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartParticleSystems(NodeAddedEvent e, WorkingNode weapon) => weapon.streamEffect.Instance.Play();

        [OnEventFire]
        public void StopParticleSystems(NodeRemoveEvent e, WorkingNode weapon) => weapon.streamEffect.Instance.Stop();

        public class StreamWeaponEffectInitNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public StreamEffectComponent streamEffect;
        }

        public class WorkingNode : Node {
            public StreamEffectComponent streamEffect;
            public StreamEffectReadyComponent streamEffectReady;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}