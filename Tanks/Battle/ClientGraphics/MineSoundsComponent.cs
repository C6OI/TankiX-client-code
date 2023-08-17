using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics {
    public class MineSoundsComponent : MonoBehaviour, Component {
        [SerializeField] AudioSource dropGroundSound;

        [SerializeField] AudioSource dropNonGroundSound;

        [SerializeField] AudioSource deactivationSound;

        [SerializeField] AudioSource explosionSound;

        public AudioSource DropGroundSound {
            get => dropGroundSound;
            set => dropGroundSound = value;
        }

        public AudioSource DropNonGroundSound {
            get => dropNonGroundSound;
            set => dropNonGroundSound = value;
        }

        public AudioSource DeactivationSound {
            get => deactivationSound;
            set => deactivationSound = value;
        }

        public AudioSource ExplosionSound {
            get => explosionSound;
            set => explosionSound = value;
        }
    }
}