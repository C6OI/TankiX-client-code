using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFallingSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] AudioSource fallingSourceAsset;

        [SerializeField] AudioClip[] fallingClips;

        [SerializeField] AudioSource collisionSourceAsset;

        [SerializeField] float minPower = 1f;

        [SerializeField] float maxPower = 64f;

        public int FallingClipIndex { get; set; }

        public AudioClip[] FallingClips {
            get => fallingClips;
            set => fallingClips = value;
        }

        public AudioSource FallingSourceAsset {
            get => fallingSourceAsset;
            set => fallingSourceAsset = value;
        }

        public AudioSource CollisionSourceAsset {
            get => collisionSourceAsset;
            set => collisionSourceAsset = value;
        }

        public float MinPower {
            get => minPower;
            set => minPower = value;
        }

        public float MaxPower {
            get => maxPower;
            set => maxPower = value;
        }

        void Awake() => FallingClipIndex = 0;
    }
}