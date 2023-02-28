using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI header;

        [SerializeField] TextMeshProUGUI gameSettings;

        [SerializeField] TextMeshProUGUI soundSettings;

        [SerializeField] TextMeshProUGUI languageSettings;

        [SerializeField] TextMeshProUGUI graphicsSettings;

        [SerializeField] TextMeshProUGUI keyboardSettings;

        public new virtual string Header {
            set => header.text = value;
        }

        public virtual string GameSettings {
            set => gameSettings.text = value;
        }

        public virtual string SoundSettings {
            set => soundSettings.text = value;
        }

        public virtual string LanguageSettings {
            set => languageSettings.text = value;
        }

        public virtual string GraphicsSettings {
            set => graphicsSettings.text = value;
        }

        public virtual string KeyboardSettings {
            set => keyboardSettings.text = value;
        }
    }
}