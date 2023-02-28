using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PersonalDiscountUIComponent : TextTimerComponent, Component {
        [SerializeField] TextMeshProUGUI description;

        public string Description {
            set => description.text = value;
        }
    }
}