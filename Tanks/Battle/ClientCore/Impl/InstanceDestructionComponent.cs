using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635824352950915226L)]
    public class InstanceDestructionComponent : Component {
        public InstanceDestructionComponent() { }

        public InstanceDestructionComponent(GameObject gameObject) => GameObject = gameObject;

        public GameObject GameObject { get; set; }
    }
}