using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class SpawnPointComponent : Component {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
    }
}