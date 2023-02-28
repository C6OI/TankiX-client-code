using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class SelectLocaleScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI hint;

        [SerializeField] Text currentLanguage;

        public string Hint {
            set => hint.text = value;
        }

        public string CurrentLanguage {
            set => currentLanguage.text = value;
        }
    }
}