using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DroneFlySoundEffectSystem : ECSSystem {
        const float MIN_VELOCITY = 0.1f;

        [OnEventFire]
        public void PlayDroneFlySound(NodeAddedEvent e, DroneEffectNode drone) {
            drone.Entity.AddComponent<DroneFlySoundEffectReadyComponent>();
            drone.droneFlySoundEffect.Sound.FadeIn();
        }

        [OnEventFire]
        public void RemoveEffect(RemoveEffectEvent e, ReadyDroneEffectNode drone) {
            drone.Entity.RemoveComponent<DroneFlySoundEffectReadyComponent>();
            drone.droneFlySoundEffect.Sound.FadeOut();
        }

        public class DroneEffectNode : Node {
            public DroneEffectComponent droneEffect;

            public DroneFlySoundEffectComponent droneFlySoundEffect;

            public RigidbodyComponent rigidbody;
        }

        public class ReadyDroneEffectNode : DroneEffectNode {
            public DroneFlySoundEffectReadyComponent droneFlySoundEffectReady;
        }
    }
}