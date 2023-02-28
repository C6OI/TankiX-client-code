using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EMPGraphicsEffectSystem : ECSSystem {
        const float REMOVE_TIME_OFFSET_SEC = 0.5f;

        const float LIGHTING_TIME_OFFSET_RELATIVE_TO_WAVE = 0.8f;

        const float INITIAL_NORMALIZED_VALUE = 0.152f;

        const float INITIAL_CURVE_SCALAR = 50f;

        const float EMP_HIT_REMOVE_TIMEOUT = 2f;

        [OnEventFire]
        public void PlayEMPEffect(EMPEffectReadyEvent e, EMPEffectNode effect, [JoinByTank] TankNode tank, [JoinAll] SingleNode<MapInstanceComponent> map) {
            EMPWaveGraphicsBehaviour eMPWaveGraphicsBehaviour = Object.Instantiate(tank.empWaveGraphicsEffect.EMPWaveAsset, tank.tankVisualRoot.transform.position,
                tank.tankVisualRoot.transform.rotation, map.component.SceneRoot.transform);

            float radius = effect.empEffect.Radius;
            float num = radius * 2f;
            ParticleSystem waveParticleSystem = eMPWaveGraphicsBehaviour.WaveParticleSystem;
            ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = waveParticleSystem.sizeOverLifetime;
            ParticleSystem.MinMaxCurve size = sizeOverLifetime.size;
            AnimationCurve curve = size.curve;
            Keyframe key = curve.keys[0];
            key.value = num * 0.152f / 50f;
            curve.MoveKey(0, key);
            size.curveMultiplier = num;
            sizeOverLifetime.size = size;
            AudioSource waveSound = eMPWaveGraphicsBehaviour.WaveSound;
            waveSound.transform.parent = null;
            waveSound.Play();
            waveParticleSystem.Play();
            Object.Destroy(eMPWaveGraphicsBehaviour.gameObject, waveParticleSystem.startLifetime + 0.5f);
            Object.Destroy(waveSound.gameObject, waveSound.clip.length + 0.5f);
        }

        [OnEventFire]
        public void PlayEMPEffect(EMPEffectReadyEvent evt, [Combine] TankNode tank) {
            NewEvent<PlayEMPHitTargetTankEvent>().Attach(tank).ScheduleDelayed(tank.empWaveGraphicsEffect.EMPWaveAsset.WaveParticleSystem.startLifetime * 0.8f);
        }

        [OnEventFire]
        public void PlayEMPEffect(PlayEMPHitTargetTankEvent e, TankNode tank) {
            ParticleSystem particleSystem = Object.Instantiate(tank.empHitVisualEffect.EmpHitPrefab, tank.tankSoundRoot.SoundRootTransform.position,
                tank.tankSoundRoot.SoundRootTransform.rotation, tank.tankSoundRoot.SoundRootTransform.transform);

            ParticleSystem.ShapeModule shape = particleSystem.shape;
            shape.mesh = tank.baseRenderer.Mesh;
            Object.Destroy(particleSystem.gameObject, 2f);
        }

        public class EMPEffectNode : Node {
            public EMPEffectComponent empEffect;

            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public EMPHitVisualEffectComponent empHitVisualEffect;

            public EMPWaveGraphicsEffectComponent empWaveGraphicsEffect;

            public TankGroupComponent tankGroup;

            public TankSoundRootComponent tankSoundRoot;

            public TankVisualRootComponent tankVisualRoot;
        }
    }
}