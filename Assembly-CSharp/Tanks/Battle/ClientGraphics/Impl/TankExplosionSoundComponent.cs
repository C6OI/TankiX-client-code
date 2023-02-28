using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankExplosionSoundComponent : Component {
        public TankExplosionSoundComponent() { }

        public TankExplosionSoundComponent(AudioSource sound) => Sound = sound;

        public AudioSource Sound { get; set; }
    }
}