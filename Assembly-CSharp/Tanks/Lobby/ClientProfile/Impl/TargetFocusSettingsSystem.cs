using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class TargetFocusSettingsSystem : ECSSystem {
        public static readonly string TARGET_FOCUS_ENABLED_KEY = "TARGET_FOCUS_ENABLED";

        public static readonly int TARGET_FOCUS_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent evt, SingleNode<TargetFocusSettingsComponent> gameSettings) {
            gameSettings.component.Enabled = IsTargetFocusEnabledInPlayerPrefs();
        }

        bool IsTargetFocusEnabledInPlayerPrefs() {
            if (!PlayerPrefs.HasKey(TARGET_FOCUS_ENABLED_KEY)) {
                PlayerPrefs.SetInt(TARGET_FOCUS_ENABLED_KEY, TARGET_FOCUS_ENABLED_DEFAULT_VALUE);
                return true;
            }

            return PlayerPrefs.GetInt(TARGET_FOCUS_ENABLED_KEY) > 0;
        }

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<TargetFocusSettingsComponent> e, SingleNode<TargetFocusSettingsComponent> settings) {
            PlayerPrefs.SetInt(TARGET_FOCUS_ENABLED_KEY, settings.component.Enabled ? 1 : 0);
        }
    }
}