using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SoundSettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI sFXVolume;

        [SerializeField] TextMeshProUGUI musicVolume;

        [SerializeField] TextMeshProUGUI uIVolume;

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