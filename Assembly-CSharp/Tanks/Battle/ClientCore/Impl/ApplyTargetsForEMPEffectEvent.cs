using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636250863918020313L)]
    public class ApplyTargetsForEMPEffectEvent : Event {
        public Entity[] Targets { get; set; }
    }
}