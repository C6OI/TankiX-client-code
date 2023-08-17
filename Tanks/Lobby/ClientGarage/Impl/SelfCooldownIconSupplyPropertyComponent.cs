using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelfCooldownIconSupplyPropertyComponent : MonoBehaviour, Component {
        [SerializeField] public GameObject RepairIcon;

        [SerializeField] public GameObject DamageIcon;

        [SerializeField] public GameObject ArmorIcon;

        [SerializeField] public GameObject SpeedIcon;

        [SerializeField] public GameObject MineIcon;
    }
}