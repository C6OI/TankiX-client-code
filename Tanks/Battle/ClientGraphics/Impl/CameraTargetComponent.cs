using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraTargetComponent : Component {
        public CameraTargetComponent() { }

        public CameraTargetComponent(GameObject targetObject) => TargetObject = targetObject;

        public GameObject TargetObject { get; set; }
    }
}