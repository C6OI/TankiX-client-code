using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankJumpSoundComponent : Component {
        public TankJumpSoundComponent() { }

        public TankJumpSoundComponent(AudioSource sound) => Sound = sound;

        public AudioSource Sound { get; set; }
    }
}