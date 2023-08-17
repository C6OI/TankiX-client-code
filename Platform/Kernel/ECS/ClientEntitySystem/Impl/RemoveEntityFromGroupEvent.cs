using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1431504381185L)]
    public class RemoveEntityFromGroupEvent : Event {
        public RemoveEntityFromGroupEvent(Type groupComponentType) => GroupComponentType = groupComponentType;

        public Type GroupComponentType { get; private set; }
    }
}