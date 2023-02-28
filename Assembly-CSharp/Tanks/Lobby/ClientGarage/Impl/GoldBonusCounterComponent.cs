using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBonusCounterComponent : BehaviourComponent {
        [SerializeField] Text count;

        public void SetCount(long count) {
            this.count.text = count.ToString();
        }
    }
}