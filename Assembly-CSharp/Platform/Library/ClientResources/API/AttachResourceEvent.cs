using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Library.ClientResources.API {
    public class AttachResourceEvent : Event {
        public Object Data { get; set; }

        public string Name { get; set; }
    }
}