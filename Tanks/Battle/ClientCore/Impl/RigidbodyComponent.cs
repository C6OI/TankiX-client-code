using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635824352590065226L)]
    public class RigidbodyComponent : Component {
        public RigidbodyComponent() { }

        public RigidbodyComponent(Rigidbody rigidbody) => Rigidbody = rigidbody;

        public Rigidbody Rigidbody { get; set; }
    }
}