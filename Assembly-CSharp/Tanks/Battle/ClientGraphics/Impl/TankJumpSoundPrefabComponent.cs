using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankJumpSoundPrefabComponent : MonoBehaviour, Component {
        public AudioSource Sound;
    }
}