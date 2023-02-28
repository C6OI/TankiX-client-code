using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Lobby.ClientSettings.API {
    public class SoundListenerResourcesBehaviour : MonoBehaviour {
        [SerializeField] AudioMixer sfxMixer;

        [SerializeField] AudioMixer musicMixer;

        [SerializeField] AudioMixer uiMixer;

        [SerializeField] AudioMixerSnapshot[] sfxMixerSnapshots;

        [SerializeField] AudioMixerSnapshot[] musicMixerSnapshots;

        [SerializeField] AudioSource sfxSourcePreview;

        [SerializeField] GameObject moduleActivation;

        [SerializeField] GameObject moduleUpgrade;

        [SerializeField] DailyBonusSoundsBehaviour dailyBonusSounds;

        public AudioMixer SfxMixer => sfxMixer;

        public AudioMixer MusicMixer => musicMixer;

        public AudioMixer UIMixer => uiMixer;

        public AudioMixerSnapshot[] SfxMixerSnapshots => sfxMixerSnapshots;

        public AudioSource SfxSourcePreview => sfxSourcePreview;

        public AudioMixerSnapshot[] MusicMixerSnapshots => musicMixerSnapshots;

        public GameObject ModuleActivation => moduleActivation;

        public GameObject ModuleUpgrade => moduleUpgrade;

        public DailyBonusSoundsBehaviour DailyBonusSounds => dailyBonusSounds;
    }
}