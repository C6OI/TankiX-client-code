using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class VolumeChangedNotifierComponent : BehaviourComponent, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler {
        [SerializeField] Slider slider;

        [SerializeField] AudioSource audioSource;

        public Slider Slider {
            get => slider;
            set => slider = value;
        }

        public AudioSource AudioSource {
            get => audioSource;
            set => audioSource = value;
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) {
            if (!slider.minValue.Equals(slider.value)) {
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat(SoundSettingsUtils.VOLUME_PARAM_KEY, slider.value);
                audioSource.Play();
            }
        }
    }
}