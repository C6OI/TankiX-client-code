using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PlayScreenSeasonGUIComponent : TextTimerComponent, Component {
        [SerializeField] LocalizedField seasonNumberTextLocalization;

        [SerializeField] TextMeshProUGUI seasonNumberText;

        public void SetSeasonNameFromNumber(long number) {
            seasonNumberText.text = string.Format(seasonNumberTextLocalization.Value, number);
        }

        public void SetSeasonName(string seasonName) {
            seasonNumberText.text = seasonName;
        }
    }
}