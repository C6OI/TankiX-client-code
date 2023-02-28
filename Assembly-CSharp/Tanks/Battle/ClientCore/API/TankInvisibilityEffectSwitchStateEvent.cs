using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class TankInvisibilityEffectSwitchStateEvent : Event {
        public TankInvisibilityEffectSwitchStateEvent(Type stateType) => StateType = stateType;

        public Type StateType { get; set; }
    }

    public class TankInvisibilityEffectSwitchStateEvent<T> : TankInvisibilityEffectSwitchStateEvent where T : Node {
        public TankInvisibilityEffectSwitchStateEvent()
            : base(typeof(T)) { }
    }
}