using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class MountPointComponent : MonoBehaviour, Component {
        public Transform MountPoint;
    }
}