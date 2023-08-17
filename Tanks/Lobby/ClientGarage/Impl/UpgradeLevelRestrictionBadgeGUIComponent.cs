using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeLevelRestrictionBadgeGUIComponent : MonoBehaviour, Component {
        [SerializeField] Text restrictionValueText;

        public string RestrictionValue {
            set => restrictionValueText.text = value;
        }
    }
}