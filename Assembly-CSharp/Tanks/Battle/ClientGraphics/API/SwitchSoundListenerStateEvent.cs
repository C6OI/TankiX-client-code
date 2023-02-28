using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class SwitchSoundListenerStateEvent : Event {
        public SwitchSoundListenerStateEvent(Type stateType) => StateType = stateType;

        public Type StateType { get; set; }
    }

    public class SwitchSoundListenerStateEvent<T> : SwitchSoundListenerStateEvent where T : Node {
        public SwitchSoundListenerStateEvent()
            : base(typeof(T)) { }
    }
}