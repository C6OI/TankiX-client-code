using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class ShellInstanceComponent : Component {
        public ShellInstanceComponent() { }

        public ShellInstanceComponent(GameObject shellInstance) => ShellInstance = shellInstance;

        public GameObject ShellInstance { get; set; }
    }
}