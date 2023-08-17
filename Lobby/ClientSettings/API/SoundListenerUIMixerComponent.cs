using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.Audio;

namespace Lobby.ClientSettings.API {
    public class SoundListenerUIMixerComponent : BehaviourComponent {
        [SerializeField] AudioMixer uiMixer;

        public AudioMixer UIMixer => uiMixer;
    }
}