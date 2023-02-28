using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserCardsUIComponent : UIBehaviour, Component {
        [SerializeField] Text[] resourceCountTexts;

        public void SetCardsCount(long type, long count) {
            int num = 0;
            resourceCountTexts[num].text = count.ToString();
        }

        public void ResetCardsCount() {
            SetCardsCount(0L, 0L);
        }
    }
}