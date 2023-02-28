using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BonusSoundConfigComponent : MonoBehaviour, Component {
        [SerializeField] AudioSource bonusTakingSound;

        public AudioSource BonusTakingSound {
            get => bonusTakingSound;
            set => bonusTakingSound = value;
        }
    }
}