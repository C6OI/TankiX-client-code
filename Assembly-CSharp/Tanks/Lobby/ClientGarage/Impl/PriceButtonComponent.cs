using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PriceButtonComponent : MonoBehaviour, Component {
        public PriceButtonComponent() { }

        public PriceButtonComponent(long price) => Price = price;

        public long Price { get; set; }
    }
}