using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeButtonComponent : LocalizedControl, Component {
        [SerializeField] Text upgradeText;

        public string UpgradeText {
            set => upgradeText.text = value;
        }
    }
}