using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class CursorStateComponent : Component {
        public CursorLockMode CursorLockState { get; set; }

        public bool CursorVisible { get; set; } = true;
    }
}