using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.Hud.Impl {
    [RequireComponent(typeof(Animator))]
    public class MineItemButtonComponent : MonoBehaviour, Component { }
}