using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class AnimatedLogoSoundBehaviour : MonoBehaviour {
        [SerializeField] AudioSource source;

        void Start() {
            source.Play();
        }
    }
}