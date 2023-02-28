using UnityEngine;

namespace Tanks.Lobby.ClientControls.Impl {
    public class AnimationSoundPlayerBehaviour : MonoBehaviour {
        public AudioSource audio;

        public void PlaySound() {
            audio.Play();
        }
    }
}