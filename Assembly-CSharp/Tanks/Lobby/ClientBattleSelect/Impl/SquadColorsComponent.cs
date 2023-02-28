using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadColorsComponent : BehaviourComponent {
        [SerializeField] Color selfSquadColor;

        [SerializeField] Color[] friendlyColor;

        [SerializeField] Color[] enemyColor;

        public Color SelfSquadColor => selfSquadColor;

        public Color[] FriendlyColor => friendlyColor;

        public Color[] EnemyColor => enemyColor;
    }
}