using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class BattleResultRewardCheckComponent : BehaviourComponent {
        [SerializeField] long quickBattleEndTutorialId;

        public long QuickBattleEndTutorialId => quickBattleEndTutorialId;
    }
}