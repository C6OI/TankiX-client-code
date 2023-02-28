using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class VisualMountPointComponent : Component {
        public Transform MountPoint { get; set; }
    }
}