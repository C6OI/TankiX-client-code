using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientUnityIntegration.API {
    public class AttachToScreenComponent : MonoBehaviour, Component {
        public EntityBehaviour JoinEntityBehaviour { get; set; }
    }
}