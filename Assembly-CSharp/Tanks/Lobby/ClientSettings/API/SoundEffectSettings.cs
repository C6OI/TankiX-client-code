using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    [RequireComponent(typeof(AudioSource))]
    public class SoundEffectSettings : MonoBehaviour {
        [SerializeField] AudioSource source;

        public AudioSource Source => source;
    }
}