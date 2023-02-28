using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class LaserSightSettingsSystem : ECSSystem {
        public static readonly string LASER_SIGHT_ENABLED_KEY = "LASER_SIGHT_ENABLED";

        public static readonly int LASER_SIGHT_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent evt, SingleNode<LaserSightSettingsComponent> gameSettings) {
            gameSettings.component.Enabled = IsLaserSightEnabledInPlayerPrefs();
        }

        bool IsLaserSightEnabledInPlayerPrefs() {
            if (!PlayerPrefs.HasKey(LASER_SIGHT_ENABLED_KEY)) {
                PlayerPrefs.SetInt(LASER_SIGHT_ENABLED_KEY, LASER_SIGHT_ENABLED_DEFAULT_VALUE);
                return true;
            }

            return PlayerPrefs.GetInt(LASER_SIGHT_ENABLED_KEY) > 0;
        }

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<LaserSightSettingsComponent> e, SingleNode<LaserSightSettingsComponent> settings) {
            PlayerPrefs.SetInt(LASER_SIGHT_ENABLED_KEY, settings.component.Enabled ? 1 : 0);
        }
    }
}