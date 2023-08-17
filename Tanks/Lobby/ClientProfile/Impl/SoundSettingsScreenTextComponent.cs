using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SoundSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text sFXVolume;

        [SerializeField] Text musicVolume;

        [SerializeField] Text uIVolume;

        public string SFXVolume {
            set => sFXVolume.text = value;
        }

        public string MusicVolume {
            set => musicVolume.text = value;
        }

        public string UIVolume {
            set => uIVolume.text = value;
        }
    }
}