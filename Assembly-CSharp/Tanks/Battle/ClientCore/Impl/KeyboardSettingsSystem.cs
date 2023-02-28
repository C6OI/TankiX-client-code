using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class KeyboardSettingsSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void ResetToDefault(ButtonClickEvent e, DefaultButtonNode button, [JoinByScreen] ScreenNode screen) {
            InputManager.ResetToDefaultActions();
            ScheduleEvent<SetDefaultControlSettingsEvent>(button);
            KeyboardSettingsInputComponent[] array = Object.FindObjectsOfType<KeyboardSettingsInputComponent>();
            KeyboardSettingsInputComponent[] array2 = array;

            foreach (KeyboardSettingsInputComponent keyboardSettingsInputComponent in array2) {
                keyboardSettingsInputComponent.SetText();
            }

            screen.keyboardSettingsScreen.CheckForOneKeyOnFewActions();
        }

        public class DefaultButtonNode : Node {
            public DefaultButtonComponent defaultButton;

            public ScreenGroupComponent screenGroup;
        }

        public class ScreenNode : Node {
            public KeyboardSettingsScreenComponent keyboardSettingsScreen;

            public ScreenGroupComponent screenGroup;
        }
    }
}