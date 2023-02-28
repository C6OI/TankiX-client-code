using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankEngineSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitTankEngineSoundEffect(NodeAddedEvent evt, [Combine] SelfTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            InitTankEngineSoundEffect(tank, true);
        }

        [OnEventFire]
        public void InitTankEngineSoundEffect(NodeAddedEvent evt, [Combine] RemoteTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            InitTankEngineSoundEffect(tank, false);
        }

        void InitTankEngineSoundEffect(InitialTankEngineSoundEffectNode tank, bool self) {
            GameObject enginePrefab = tank.tankEngineSoundEffect.EnginePrefab;
            GameObject gameObject = Object.Instantiate(enginePrefab);
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            gameObject.transform.parent = soundRootTransform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            HullSoundEngineController component = gameObject.GetComponent<HullSoundEngineController>();
            component.Init(self);
            tank.tankEngineSoundEffect.SoundEngineController = component;
            tank.Entity.AddComponent<TankEngineSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartEngine(NodeAddedEvent evt, TankEngineSoundEffectReadyNode tank) {
            tank.tankEngineSoundEffect.SoundEngineController.Play();
        }

        [OnEventFire]
        public void UpdateEngine(UpdateEvent evt, TankEngineSoundEffectReadyNode tank) {
            tank.tankEngineSoundEffect.SoundEngineController.InputRpmFactor = tank.tankEngine.Value;
        }

        [OnEventFire]
        public void StopEngine(NodeRemoveEvent evt, TankEngineSoundEffectReadyNode tank) {
            tank.tankEngineSoundEffect.SoundEngineController.Stop();
        }

        public class InitialTankEngineSoundEffectNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankEngineComponent tankEngine;

            public TankEngineSoundEffectComponent tankEngineSoundEffect;

            public TankSoundRootComponent tankSoundRoot;
        }

        public class SelfTankNode : InitialTankEngineSoundEffectNode {
            public SelfTankComponent selfTank;
        }

        public class RemoteTankNode : InitialTankEngineSoundEffectNode {
            public RemoteTankComponent remoteTank;
        }

        public class TankEngineSoundEffectReadyNode : Node {
            public TankEngineComponent tankEngine;

            public TankEngineSoundEffectComponent tankEngineSoundEffect;

            public TankEngineSoundEffectReadyComponent tankEngineSoundEffectReady;

            public TankMovableComponent tankMovable;
        }
    }
}