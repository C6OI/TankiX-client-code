using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class XCrystalsMethodSaleLabelComponent : LocalizedControl, Component {
        [SerializeField] Text timerText;

        public string Text {
            set => timerText.text = value;
        }
    }
}