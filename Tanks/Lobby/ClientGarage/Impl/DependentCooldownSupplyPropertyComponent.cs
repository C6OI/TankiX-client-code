using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DependentCooldownSupplyPropertyComponent : MonoBehaviour, Component {
        [SerializeField] public SupplyType supplyType;
    }
}