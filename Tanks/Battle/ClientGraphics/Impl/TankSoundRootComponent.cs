using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankSoundRootComponent : Component {
        public TankSoundRootComponent() { }

        public TankSoundRootComponent(Transform soundRootTransform) => SoundRootTransform = soundRootTransform;

        public Transform SoundRootTransform { get; set; }
    }
}