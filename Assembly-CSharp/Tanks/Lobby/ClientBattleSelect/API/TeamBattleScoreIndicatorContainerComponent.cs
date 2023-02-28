using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class TeamBattleScoreIndicatorContainerComponent : MonoBehaviour, Component {
        [SerializeField] GameObject TDMScoreIndicator;

        [SerializeField] GameObject CTFScoreIndicator;

        public GameObject TdmScoreIndicator {
            get => TDMScoreIndicator;
            set => TDMScoreIndicator = value;
        }

        public GameObject CtfScoreIndicator {
            get => CTFScoreIndicator;
            set => CTFScoreIndicator = value;
        }
    }
}