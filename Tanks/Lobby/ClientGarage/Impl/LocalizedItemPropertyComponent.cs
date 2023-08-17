using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LocalizedItemPropertyComponent : MonoBehaviour, Component {
        [SerializeField] Text propertyName;

        public string PropertyName {
            set => propertyName.text = value;
        }
    }
}