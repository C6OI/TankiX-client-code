using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class TanksSettingsSystem : ECSSystem {
        public static readonly string MOVEMENT_CONTROL_INVERTED_PP_KEY = "MOVEMENT_CONTROL_INVERTED";

        public static readonly int MOVEMENT_CONTROL_INVERTED_DEFAULT_VALUE;

        [OnEventFire]
        public void InitGameSettings(NodeAddedEvent e, SingleNode<GameTankSettingsComponent> gameSettings) =>
            gameSettings.component.MovementControlsInverted =
                PlayerPrefs.GetInt(MOVEMENT_CONTROL_INVERTED_PP_KEY, MOVEMENT_CONTROL_INVERTED_DEFAULT_VALUE) > 0;

        [OnEventFire]
        public void GameSettingsChanged(SettingsChangedEvent<GameTankSettingsComponent> e, Node any) =>
            PlayerPrefs.SetInt(MOVEMENT_CONTROL_INVERTED_PP_KEY, e.Data.MovementControlsInverted ? 1 : 0);
    }
}