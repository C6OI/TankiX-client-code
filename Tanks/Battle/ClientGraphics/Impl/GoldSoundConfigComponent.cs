using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GoldSoundConfigComponent : MonoBehaviour, Component {
        [SerializeField] AudioSource goldNotificationSound;

        public AudioSource GoldNotificationSound {
            get => goldNotificationSound;
            set => goldNotificationSound = value;
        }
    }
}