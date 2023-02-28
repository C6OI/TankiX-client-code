using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    public class MineItemButtonComponent : MonoBehaviour, Component { }
}