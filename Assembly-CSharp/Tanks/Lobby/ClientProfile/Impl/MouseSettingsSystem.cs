using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class MouseSettingsSystem : ECSSystem {
        static readonly string MOUSE_CONTROL_ALLOWED_PP_KEY = "MOUSE_CONTROL_ALLOWED";

        static readonly int MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE = 1;

        static readonly string MOUSE_VERTICAL_INVERTED_PP_KEY = "MOUSE_VERTICAL_INVERTED";

        static readonly int MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE;

        static readonly string MOUSE_SENSIVITY_PP_KEY = "MOUSE_SENSIVITY";

        static readonly float MOUSE_SENSIVITY_DEFAULT_VALUE = 0.5f;

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent e, SingleNode<GameMouseSettingsComponent> gameSettings) {
            gameSettings.component.MouseControlAllowed = PlayerPrefs.GetInt(MOUSE_CONTROL_ALLOWED_PP_KEY, MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE) > 0;
            gameSettings.component.MouseVerticalInverted = PlayerPrefs.GetInt(MOUSE_VERTICAL_INVERTED_PP_KEY, MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE) > 0;
            gameSettings.component.MouseSensivity = PlayerPrefs.GetFloat(MOUSE_SENSIVITY_PP_KEY, MOUSE_SENSIVITY_DEFAULT_VALUE);
        }

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameMouseSettingsComponent> e, Node any) {
            PlayerPrefs.SetInt(MOUSE_CONTROL_ALLOWED_PP_KEY, e.Data.MouseControlAllowed ? 1 : 0);
            PlayerPrefs.SetInt(MOUSE_VERTICAL_INVERTED_PP_KEY, e.Data.MouseVerticalInverted ? 1 : 0);
            PlayerPrefs.SetFloat(MOUSE_SENSIVITY_PP_KEY, e.Data.MouseSensivity);
        }

        [OnEventFire]
        public void SetDefaultMouseSettings(SetDefaultControlSettingsEvent e, Node any, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseControlAllowed = MOUSE_CONTROL_ALLOWED_DEFAULT_VALUE == 1;
            settings.component.MouseVerticalInverted = MOUSE_VERTICAL_INVERTED_DEFAULT_VALUE == 1;
            settings.component.MouseSensivity = MOUSE_SENSIVITY_DEFAULT_VALUE;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }
    }
}