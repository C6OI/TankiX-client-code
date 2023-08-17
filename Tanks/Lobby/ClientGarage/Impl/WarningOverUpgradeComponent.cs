using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(635894159848027950L)]
    public class WarningOverUpgradeComponent : MonoBehaviour, Component {
        [SerializeField] Text warningText;

        public string WarningText {
            set => warningText.text = value;
        }
    }
}