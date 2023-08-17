using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class BattleDetailsIndicatorsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject scoreIndicator;

        [SerializeField] GameObject timeIndicator;

        [SerializeField] BattleLevelsIndicatorComponent battleLevelsIndicator;

        [SerializeField] LevelWarningComponent levelWarning;

        [SerializeField] GameObject archivedBattleIndicator;

        public GameObject ScoreIndicator => scoreIndicator;

        public GameObject TimeIndicator => timeIndicator;

        public BattleLevelsIndicatorComponent BattleLevelsIndicator => battleLevelsIndicator;

        public LevelWarningComponent LevelWarning => levelWarning;

        public GameObject ArchivedBattleIndicator => archivedBattleIndicator;
    }
}