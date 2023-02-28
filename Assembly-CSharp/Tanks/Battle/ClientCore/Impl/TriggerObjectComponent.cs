using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class TriggerObjectComponent : Component {
        public TriggerObjectComponent() { }

        public TriggerObjectComponent(GameObject triggerObject) => TriggerObject = triggerObject;

        public GameObject TriggerObject { get; set; }
    }
}