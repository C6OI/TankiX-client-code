using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public abstract class RestrictionDescriptionGUIComponent : MonoBehaviour, Component {
        [SerializeField] Text description;

        public string Description {
            set => description.text = value;
        }
    }
}