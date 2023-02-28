using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PersonalXCrystalBonusUIComponent : LocalizedControl, Component {
        [SerializeField] TextMeshProUGUI description;

        public string Description {
            set => description.text = value;
        }
    }
}