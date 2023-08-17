using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientSettings.Impl {
    public class SelectLocaleScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text hint;

        [SerializeField] Text currentLanguage;

        public string Hint {
            set => hint.text = value;
        }

        public string CurrentLanguage {
            set => currentLanguage.text = value;
        }
    }
}