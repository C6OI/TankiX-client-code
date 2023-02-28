using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class SubscribeCheckboxLocalizedStringsComponent : FromConfigBehaviour, Component {
        [SerializeField] TextMeshProUGUI subscribeLine1Text;

        [SerializeField] TextMeshProUGUI subscribeLine2Text;

        public string SubscribeLine1Text {
            set => subscribeLine1Text.text = value;
        }

        public string SubscribeLine2Text {
            set => subscribeLine2Text.text = value;
        }

        protected override string GetRelativeConfigPath() => "/ui/element";
    }
}