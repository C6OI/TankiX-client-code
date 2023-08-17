using UnityEngine;
using UnityEngine.Audio;

namespace Lobby.ClientSettings.API {
    public class SoundListenerResourcesBehaviour : MonoBehaviour {
        [SerializeField] AudioMixer sfxMixer;

        [SerializeField] AudioMixer musicMixer;

        [SerializeField] AudioMixerSnapshot[] sfxMixerSnapshots;

        [SerializeField] AudioSource sfxSourcePreview;

        public AudioMixer SfxMixer => sfxMixer;

        public AudioMixer MusicMixer => musicMixer;

        public AudioMixerSnapshot[] SfxMixerSnapshots => sfxMixerSnapshots;

        public AudioSource SfxSourcePreview => sfxSourcePreview;
    }
}