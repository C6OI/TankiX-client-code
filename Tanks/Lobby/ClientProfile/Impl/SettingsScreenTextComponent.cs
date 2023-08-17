using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SettingsScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text gameSettings;

        [SerializeField] Text soundSettings;

        [SerializeField] Text languageSettings;

        [SerializeField] Text graphicsSettings;

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
    }
}