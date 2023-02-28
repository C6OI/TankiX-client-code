using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankOutlineMapEffectSwitchStateEvent : Event {
        public TankOutlineMapEffectSwitchStateEvent(Type stateType) => StateType = stateType;

        public Type StateType { get; set; }
    }

    public class TankOutlineMapEffectSwitchStateEvent<T> : TankOutlineMapEffectSwitchStateEvent where T : Node {
        public TankOutlineMapEffectSwitchStateEvent()
            : base(typeof(T)) { }
    }
}