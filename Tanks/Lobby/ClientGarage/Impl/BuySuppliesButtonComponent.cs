using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuySuppliesButtonComponent : MonoBehaviour, Component {
        public int supplyCount;

        public BuySuppliesButtonComponent() { }

        public BuySuppliesButtonComponent(int supplyCount) => this.supplyCount = supplyCount;
    }
}