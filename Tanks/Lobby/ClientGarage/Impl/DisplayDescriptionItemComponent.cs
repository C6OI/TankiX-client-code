using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplayDescriptionItemComponent : MonoBehaviour, Component {
        [SerializeField] TopTextDescriptionItem description;

        public void SetDescription(string text) => description.text = text;
    }
}