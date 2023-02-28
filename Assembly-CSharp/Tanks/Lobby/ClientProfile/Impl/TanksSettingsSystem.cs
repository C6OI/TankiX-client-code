using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class TanksSettingsSystem : ECSSystem {
        const string MOVEMENT_CONTROL_INVERTED_PP_KEY = "MOVEMENT_CONTROL_INVERTED";

        const int MOVEMENT_CONTROL_INVERTED_DEFAULT_VALUE = 0;

        const string DAMAGE_INFO_ENABLED_PP_KEY = "DAMAGE_INFO_ENABLED";

        const int DAMAGE_INFO_ENABLED_DEFAULT_VALUE = 1;

        const string HEALTH_FEEDBACK_ENABLED_PP_KEY = "LOW_HEALTH_FEEDBACK_ENABLED";

        const int HEALTH_FEEDBACK_ENABLED_DEFAULT_VALUE = 1;

        const string SELF_TARGET_HIT_FEEDBACK_ENABLED_PP_KEY = "SELF_TARGET_HIT_FEEDBACK_ENABLED";

        const int SELF_TARGET_HIT_FEEDBACK_ENABLED_DEFAULT_VALUE = 1;

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent e, SingleNode<GameTankSettingsComponent> gameSettings, SingleNode<FeedbackGraphicsRestrictionsComponent> qualitySettings) {
            gameSettings.component.MovementControlsInverted = PlayerPrefs.GetInt("MOVEMENT_CONTROL_INVERTED", 0) > 0;
            gameSettings.component.DamageInfoEnabled = PlayerPrefs.GetInt("DAMAGE_INFO_ENABLED", 1) > 0;
            gameSettings.component.HealthFeedbackEnabled = PlayerPrefs.GetInt("LOW_HEALTH_FEEDBACK_ENABLED", 1) > 0 && qualitySettings.component.HealthFeedbackAllowed;
            gameSettings.component.SelfTargetHitFeedbackEnabled = PlayerPrefs.GetInt("SELF_TARGET_HIT_FEEDBACK_ENABLED", 1) > 0 && qualitySettings.component.SelfTargetHitFeedbackAllowed;
        }

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameTankSettingsComponent> e, Node any, [JoinAll] SingleNode<FeedbackGraphicsRestrictionsComponent> quality) {
            PlayerPrefs.SetInt("MOVEMENT_CONTROL_INVERTED", e.Data.MovementControlsInverted ? 1 : 0);
            PlayerPrefs.SetInt("DAMAGE_INFO_ENABLED", e.Data.DamageInfoEnabled ? 1 : 0);

            if (quality.component.SelfTargetHitFeedbackAllowed) {
                PlayerPrefs.SetInt("SELF_TARGET_HIT_FEEDBACK_ENABLED", e.Data.SelfTargetHitFeedbackEnabled ? 1 : 0);
            }

            if (quality.component.HealthFeedbackAllowed) {
                PlayerPrefs.SetInt("LOW_HEALTH_FEEDBACK_ENABLED", e.Data.HealthFeedbackEnabled ? 1 : 0);
            }
        }

        [OnEventFire]
        public void SetDefaultMouseSettings(SetDefaultControlSettingsEvent e, Node any, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.MovementControlsInverted = false;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }
    }
}