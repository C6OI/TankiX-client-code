using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824352002755226L)]
    public class WeaponRotationSoundComponent : MonoBehaviour, Component {
        [SerializeField] GameObject asset;

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public AudioSource StartAudioSource { get; set; }

        public AudioSource LoopAudioSource { get; set; }

        public AudioSource StopAudioSource { get; set; }

        public bool IsActive { get; set; }
    }
}