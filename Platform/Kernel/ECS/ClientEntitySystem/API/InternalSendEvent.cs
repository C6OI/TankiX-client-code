using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [SerialVersionUID(1429863405598L)]
    public class InternalSendEvent : Event {
        public ICollection<Entity> Entities { get; set; }

        public Event RealRealEvent { get; set; }

        public InternalSendEvent Init(Event realEvent, ICollection<Entity> entities) {
            RealRealEvent = realEvent;
            Entities = entities;
            return this;
        }
    }
}